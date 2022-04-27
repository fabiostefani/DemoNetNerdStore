using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using WebApp.MVC.Core;
using WebApp.MVC.Models;

namespace WebApp.MVC.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("erro/{id:length(3,3)}")]
    public IActionResult Error(int id)
    {
        var modelErro = new ErrorViewModel();

        if (id == InternalStatusCode.InternalServerError)
        {
            modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
            modelErro.Titulo = "Ocorreu um erro!";
            modelErro.ErroCode = id;
        }
        else if (id == InternalStatusCode.NotFound)
        {
            modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
            modelErro.Titulo = "Ops! Página não encontrada.";
            modelErro.ErroCode = id;
        }
        else if (id == InternalStatusCode.Forbidden)
        {
            modelErro.Mensagem = "Você não tem permissão para fazer isto.";
            modelErro.Titulo = "Acesso Negado";
            modelErro.ErroCode = id;
        }
        else
        {
            return StatusCode(InternalStatusCode.NotFound);
        }

        return View("Error", modelErro);
    }

    [Route("sistema-indisponivel")]
    public IActionResult SistemaIndisponivel()
    {
        var modelErro = new ErrorViewModel
        {
            Mensagem = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários",
            Titulo = "Sistema indisponível",
            ErroCode = (int) HttpStatusCode.InternalServerError
        };
        return View("Error", modelErro);
    }
}
