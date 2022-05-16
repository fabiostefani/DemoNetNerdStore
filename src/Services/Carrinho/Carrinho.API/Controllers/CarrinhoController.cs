using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Api.Core.Controllers;
using Api.Core.Usuario;
using Carrinho.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Carrinho.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Carrinho.API.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    private readonly IAspNetUser _aspNetUser;
    private readonly CarrinhoContext _context;

    public CarrinhoController(IAspNetUser aspNetUser,
                              CarrinhoContext context)
    {
        _aspNetUser = aspNetUser;
        _context = context;
    }
    
    [HttpGet("carrinho")]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return await ObterCarrinhoCliente() ?? new CarrinhoCliente();
    }
    [HttpPost("carrinho")]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();
        if (carrinho == null)
            ManipularNovoCarrinho(item);
        else
            ManipularCarrinhoExistente(carrinho, item);
        if (!OperacaoValida()) return CustomResponse();
        var result = await _context.SaveChangesAsync();
        if (result <= 0)
            AdicionarErroProcessamento("Não foi possível persistir os dados no banco.");
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

    private async Task<CarrinhoCliente?> ObterCarrinhoCliente()
        => await _context.CarrinhoClientes
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.ClienteId == _aspNetUser.ObterUserId());
    
    private void ManipularNovoCarrinho(CarrinhoItem item)
    {
        var carrinho = new CarrinhoCliente(_aspNetUser.ObterUserId());
        carrinho.AdicionarItem(item);
        _context.CarrinhoClientes.Add(carrinho);
    }
    
    private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
    {
        bool produtoItemExistente = carrinho.CarrinhoItemExistente(item);

        carrinho.AdicionarItem(item);
        // ValidarCarrinho(carrinho);

        if (produtoItemExistente)
        {
            _context.CarrinhoItens.Update(carrinho.ObterPorProdutoId(item.ProdutoId));
        }
        else
        {
            _context.CarrinhoItens.Add(item);
        }
        _context.CarrinhoClientes.Update(carrinho);
    }
}