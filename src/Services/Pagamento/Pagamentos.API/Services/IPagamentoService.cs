using Core.Message.Integration;
using Pagamentos.API.Models;

namespace Pagamentos.API.Services;

public interface IPagamentoService
{
    Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);
}