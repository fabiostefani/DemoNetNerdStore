using BFF.Compras.Models;

namespace BFF.Compras.Services.Interfaces;

public interface IPedidoService
{
    Task<VocuherDto?> ObterVoucherPorCodigo(string codigo);
}