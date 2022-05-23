using Microsoft.AspNetCore.Mvc;
using WebApp.MVC.Models;
using WebApp.MVC.Services;

namespace WebApp.MVC.Extensions;

public class CarrinhoViewComponent : ViewComponent
{
    private readonly ICarrinhoService _carrinhoService;

    public CarrinhoViewComponent(ICarrinhoService carrinhoService)
    {
        _carrinhoService = carrinhoService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _carrinhoService.ObterCarrinho() ?? new CarrinhoViewModel());
    }
}