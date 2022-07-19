using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApp.MVC.Models;
using WebApp.MVC.Services;

namespace WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;
        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;

        }
        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            var respostaLogin = await _autenticacaoService.Registro(usuarioRegistro);            
            if (respostaLogin == null)
                throw new Exception("Erro gerando o login na API de Autenticação.");            
            if (ResponsePossuiErros(respostaLogin.ResponseResult) ) return View(usuarioRegistro);
            await _autenticacaoService.RealizarLogin(respostaLogin);

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(/*string returnUrl = null*/)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin/*, string returnUrl = null*/)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(usuarioLogin);
            var respostaLogin = await _autenticacaoService.Login(usuarioLogin);
            ArgumentNullException.ThrowIfNull(respostaLogin, "Erro gerando o login na API de Autenticação.");            
            if (ResponsePossuiErros(respostaLogin.ResponseResult) ) return View(usuarioLogin);
            await _autenticacaoService.RealizarLogin(respostaLogin);
            // if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");
            // return LocalRedirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            await _autenticacaoService.Logout();
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}