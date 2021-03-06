using Core.Communication;
using Microsoft.AspNetCore.Mvc;
using WebApp.MVC.Models;

namespace WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult? response)
        {
            ArgumentNullException.ThrowIfNull(response, "Resposta Inválida.");            
            if (!PossuiErros(response)) return false;
            response.Errors.Mensagens.ForEach
            (
                mensagem => ModelState.AddModelError(string.Empty, mensagem)
            );            
            return true;            
        }

        private bool PossuiErros(ResponseResult? response)
            => response is not null && response.Errors.Mensagens.Any();

        protected void AdicionarErroValidacao(string mensagem)
            => ModelState.AddModelError(string.Empty, mensagem);

        protected bool OperacaoValida()
            => ModelState.ErrorCount == 0;
    }
}