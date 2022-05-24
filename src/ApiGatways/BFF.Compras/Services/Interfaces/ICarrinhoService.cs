using BFF.Compras.Models;
using Core.Communication;

namespace BFF.Compras.Services.Interfaces;

public interface ICarrinhoService
{
    Task<CarrinhoDto> ObterCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDto produto);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto carrinho);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
}