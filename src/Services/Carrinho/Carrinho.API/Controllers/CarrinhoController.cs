using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Api.Core.Controllers;
using Api.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Carrinho.API.Model;

namespace Carrinho.API.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    private readonly IAspNetUser _aspNetUser;
    public CarrinhoController(IAspNetUser aspNetUser)
    {
        _aspNetUser = aspNetUser;
    }
    
    [HttpGet("carrinho")]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return null;
    }
    [HttpPost("carrinho")]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        return CustomResponse();
    }

    [HttpPut("carrinho/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        return CustomResponse();
    }

    [HttpDelete("carrinho/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        return CustomResponse();
    }
}