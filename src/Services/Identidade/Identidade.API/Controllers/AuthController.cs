using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Core.Controllers;
using Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Api.Core.Identidade;
using Api.Core.Usuario;
using Core.Message.Integration;
using EasyNetQ;
using MessageBus;
using NetDevPack.Security.Jwt.Core.Interfaces;

namespace Identidade.API.Controllers
{
    [Route("api/identidade")]
    [Produces("application/json")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IMessageBus _bus;
        private readonly IAspNetUser _aspNetUser;
        private readonly IJwtService _jwtService;

        public AuthController(SignInManager<IdentityUser> signManager,
                              UserManager<IdentityUser> userManager, 
                              IOptions<AppSettings> appSettings,
                              IMessageBus bus, 
                              IAspNetUser aspNetUser, 
                              IJwtService jwtService)
        {
            _signManager = signManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _bus = bus;
            _aspNetUser = aspNetUser;
            _jwtService = jwtService;
        }
        /// <summary>
        /// Adiciona um Novo Usuário
        /// </summary>
        /// <param name="usuarioRegistro"></param>
        /// <returns>Um novo usuário cadastrado</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth
        ///     {
        ///        "Email": "teste@teste.com",
        ///        "Senha": "Teste@1234",
        ///        "SenhaConfirmacao": "Teste@1234"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Usuário já existente no Identity</response>
        [HttpPost("nova-conta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) 
                return CustomResponse(usuarioRegistro);
            IdentityUser user = CreateUserIdentity(usuarioRegistro);
            var resultCreateUser = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
            if (resultCreateUser.Succeeded)
            {
                ResponseMessage clienteResult = await RegistrarCliente(usuarioRegistro);
                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(clienteResult.ValidationResult);
                }
                return CustomResponse(await GerarJwt(usuarioRegistro.Email));    
            }
            foreach (var erro in resultCreateUser.Errors)
            {
                AdicionarErroProcessamento(erro.Description);
            }
            return CustomResponse();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
        {
            IdentityUser? usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email);
            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario.Id), usuarioRegistro.Nome,
                usuarioRegistro.Email, usuarioRegistro.Cpf);
            try
            {
                return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch
            {
                await _userManager.DeleteAsync(usuario);
                throw;
            }
        }
        
        private static IdentityUser CreateUserIdentity(UsuarioRegistro usuarioRegistro)
        {
            return new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };
        }

        /// <summary>
        /// Faz login do Usuário
        /// </summary>
        /// <param name="usuarioLogin"></param>
        /// <returns>Token de acesso do usuário</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth
        ///     {
        ///        "Email": "teste@teste.com",
        ///        "Senha": "Teste@1234"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Usuário não autenticado no Identity</response>
        [HttpPost("autenticar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse();
            var resultLogin = await _signManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, isPersistent: false, lockoutOnFailure: true);
            if (resultLogin.Succeeded) return CustomResponse(await GerarJwt(usuarioLogin.Email));

            if (resultLogin.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário bloqueado");
                return CustomResponse();
            }
            AdicionarErroProcessamento("Usuário/senha inválido.");
            return CustomResponse();
        }

        private async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await ObterClaimsUsuario(claims, user);
            var encodedToken = await CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, user, claims);
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private async Task<string> CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = await _jwtService.GetCurrentSigningCredentials();
            var currentIssuer = $"{_aspNetUser.ObterHttpContext().Request.Scheme}://{_aspNetUser.ObterHttpContext().Request.Host}";

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = key
            });

            return tokenHandler.WriteToken(token);            
        }

        private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, ICollection<Claim> claims)
        {
            var response = new UsuarioRespostaLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(x => new UsuarioClaim { Type = x.Type, Value = x.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}