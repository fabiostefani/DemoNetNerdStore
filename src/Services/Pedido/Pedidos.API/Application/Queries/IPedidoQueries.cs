using Pedidos.API.Application.Dtos;

namespace Pedidos.API.Application.Queries;

public interface IPedidoQueries
{
    Task<PedidoDto> ObterUltimoPedido(Guid clienteId);
    Task<IEnumerable<PedidoDto>> ObterListaPorClienteId(Guid clienteId);
}