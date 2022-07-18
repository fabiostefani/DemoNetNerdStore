using Api.Core.Controllers;
using Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Message.Integration;
using Identidade.API.Services;
using MessageBus;

namespace Identidade.API.Controllers
{
    [Route("api/identidade")]
    [Produces("application/json")]
    public class AuthController : MainController
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IMessageBus _bus;

        public AuthController(
            AuthenticationService authenticationService,
            IMessageBus bus)
        {
            _authenticationService = authenticationService;
            _bus = bus;
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
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _authenticationService.UserManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                var clienteResult = await RegistrarCliente(usuarioRegistro);

                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _authenticationService.UserManager.DeleteAsync(user);
                    return CustomResponse(clienteResult.ValidationResult);
                }

                return CustomResponse(await _authenticationService.GerarJwt(usuarioRegistro.Email));
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
        {
            var usuario = await _authenticationService.UserManager.FindByEmailAsync(usuarioRegistro.Email);

            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
                Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

            try
            {
                return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch
            {
                await _authenticationService.UserManager.DeleteAsync(usuario);
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
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _authenticationService.SignInManager.PasswordSignInAsync(usuarioLogin.Email,
                usuarioLogin.Senha,
                false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await _authenticationService.GerarJwt(usuarioLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha incorretos");
            return CustomResponse();
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AdicionarErroProcessamento("Refresh Token inválido");
                return CustomResponse();
            }

            var token = await _authenticationService.ObterRefreshToken(Guid.Parse(refreshToken));

            if (token is null)
            {
                AdicionarErroProcessamento("Refresh Token expirado");
                return CustomResponse();
            }

            return CustomResponse(await _authenticationService.GerarJwt(token.Username));
        }
    }
}