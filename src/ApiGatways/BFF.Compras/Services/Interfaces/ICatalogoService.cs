using BFF.Compras.Models;

namespace BFF.Compras.Services.Interfaces;

public interface ICatalogoService
{
    Task<ItemProdutoDto> ObterPorId(Guid id);
}