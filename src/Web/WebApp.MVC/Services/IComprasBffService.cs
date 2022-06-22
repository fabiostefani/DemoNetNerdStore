using Core.Communication;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services;

public interface IComprasBffService
{
    Task<CarrinhoViewModel?> ObterCarrinho();
    Task<int> ObterQuantidadeCarrinho();
    Task<ResponseResult?> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho);
    Task<ResponseResult?> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel carrinho);
    Task<ResponseResult?> RemoverItemCarrinho(Guid produtoId);
    Task<ResponseResult?> AplicarVoucherCarrinho(string voucher);
    PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco);
    Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
    Task<PedidoViewModel> ObterUltimoPedido();
    Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
}