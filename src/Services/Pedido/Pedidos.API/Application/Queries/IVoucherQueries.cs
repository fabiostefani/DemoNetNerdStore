using Pedidos.API.Application.Dtos;

namespace Pedidos.API.Application.Queries;

public interface IVoucherQueries
{
    Task<VoucherDto?> ObterVoucherPorCodigo(string codigo);
}