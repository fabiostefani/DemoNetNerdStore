using Core.Message.Integration;
using MessageBus;
using Pagamentos.API.Models;

namespace Pagamentos.API.Services;

public class PagamentoIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly IServiceProvider _serviceProvider;

    public PagamentoIntegrationHandler(IMessageBus messageBus,
                                       IServiceProvider serviceProvider)
    {
        _messageBus = messageBus;
        _serviceProvider = serviceProvider;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private void SetResponder()
    {
        _messageBus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request =>
            await AutorizarPagamento(request));
    }

    private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
    {
        ResponseMessage response;
        using (var scope = _serviceProvider.CreateScope())
        {
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();
            var pagamento = new Pagamento()
            {
                PedidoId = message.PedidoId,
                TipoPagamento = (TipoPagamento) message.TipoPagamento,
                Valor = message.Valor,
                CartaoCredito = new CartaoCredito(message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento,
                    message.Cvv)
            };
            response = await pagamentoService.AutorizarPagamento(pagamento);
        }

        return response;
    }
}