using Microsoft.AspNetCore.Mvc;
using WebApp.MVC.Models;

namespace WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult response)
            => response != null && response.Errors.Mensagens.Any();

    }
}