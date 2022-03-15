using Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signManager, UserManager<IdentityUser> userManager)
        {
            _signManager = signManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
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

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return BadRequest();
            var resultLogin = await _signManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, isPersistent: false, lockoutOnFailure: true);
            if (resultLogin.Succeeded) return Ok();
            return BadRequest();
        }
    }
}