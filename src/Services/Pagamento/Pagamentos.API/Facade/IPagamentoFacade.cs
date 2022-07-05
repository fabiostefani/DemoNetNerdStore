using Pagamentos.API.Models;

namespace Pagamentos.API.Facade;

public interface IPagamentoFacade
{
    Task<Transacao> AutorizarPagamento(Pagamento pagamento);
}