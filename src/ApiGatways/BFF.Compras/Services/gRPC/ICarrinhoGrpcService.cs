using BFF.Compras.Models;

namespace BFF.Compras.Services.gRPC;

public interface ICarrinhoGrpcService
{
    Task<CarrinhoDto> ObterCarrinho();
}