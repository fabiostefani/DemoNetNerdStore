using Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identidade.API.Controllers
{
    [ApiController]
    [Route("api/identidade")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signManager, UserManager<IdentityUser> userManager)
        {
            _signManager = signManager;
            _userManager = userManager;
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
            if (!ModelState.IsValid) return BadRequest();
            IdentityUser user = CreateUserIdentity(usuarioRegistro);
            var resultCreateUser = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
            if (resultCreateUser.Succeeded)
            {
                await _signManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            return BadRequest();
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
            if (!ModelState.IsValid) return BadRequest();
            var resultLogin = await _signManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, isPersistent: false, lockoutOnFailure: true);
            if (resultLogin.Succeeded) return Ok();
            return BadRequest();
        }
    }
}