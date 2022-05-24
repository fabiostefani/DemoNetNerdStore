using Api.Core.Controllers;
using BFF.Compras.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace BFF.Compras.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly ICatalogoService _catalogoService;
    public CarrinhoController(ICarrinhoService carrinhoService,
                              ICatalogoService catalogoService)
    {
        _carrinhoService = carrinhoService;
        _catalogoService = catalogoService;
    }
    
    [HttpGet]
    [Route("compras/carrinho")]
    public async Task<IActionResult> Index()
    {
        return CustomResponse();
    }
    
    [HttpGet]
    [Route("compras/carrinho-quantidade")]
    public async Task<IActionResult> ObterQuantidadeCarrinho()
    {
        return CustomResponse();
    }
    
    [HttpPost]
    [Route("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho()
    {
        return CustomResponse();
    }
    
    [HttpPut]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId)
    {
        return CustomResponse();
    }
    
    [HttpDelete]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> DeletarItemCarrinho(Guid produtoId)
    {
        return CustomResponse();
    }

}