using Core.DomainObjects;
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
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetResponder()
    {
        _messageBus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request =>
            await AutorizarPagamento(request));
    }

    private void SetSubscribers()
    {
        _messageBus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request =>
            await CancelarPagamento(request));
        
        _messageBus.SubscribeAsync<PedidoBaixadoIntegrationEvent>("PedidoBaixadoEstoque", async request =>
            await CapturarPagamento(request));
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
    
    private async Task CapturarPagamento(PedidoBaixadoIntegrationEvent message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();
            var response = await pagamentoService.CapturarPagamento(message.PedidoId);
            if (!response.ValidationResult.IsValid)
            {
                throw new DomainException($"Falha ao capturar pagamento do pedido {message.PedidoId}");
            }

            await _messageBus.PublishAsync(new PedidoPagoIntegrationEvent(message.ClienteId, message.PedidoId));
        }
    }

    private async Task CancelarPagamento(PedidoCanceladoIntegrationEvent message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();
            var response = await pagamentoService.CancelarPagamento(message.PedidoId);
            if (!response.ValidationResult.IsValid)
            {
                throw new DomainException($"Falha ao cancelar pagamento do pedido {message.PedidoId}");
            }
        }
    }
}