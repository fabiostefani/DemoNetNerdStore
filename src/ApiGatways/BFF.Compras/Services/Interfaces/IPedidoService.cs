using BFF.Compras.Models;
using Core.Communication;

namespace BFF.Compras.Services.Interfaces;

public interface IPedidoService
{
    Task<VocuherDto?> ObterVoucherPorCodigo(string codigo);
    Task<ResponseResult> FinalizarPedido(PedidoDto pedido);
    Task<PedidoDto> ObterUltimoPedido();
    Task<IEnumerable<PedidoDto>> ObterListaPorClienteId();
}