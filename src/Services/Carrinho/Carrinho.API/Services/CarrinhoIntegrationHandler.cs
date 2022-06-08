using Carrinho.API.Data;
using Core.Message.Integration;
using MessageBus;
using Microsoft.EntityFrameworkCore;

namespace Carrinho.API.Services;

public class CarrinhoIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;
    public CarrinhoIntegrationHandler(IMessageBus bus,
                                      IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribes();
        return Task.CompletedTask;
    }

    private void SetSubscribes()
    {
        _bus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado",
            async request => await ApagarCarrinho(request));
    }

    private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();
        var carrinho = await context.CarrinhoClientes.FirstOrDefaultAsync(c => c.ClienteId == message.ClienteId);
        if (carrinho is null) return;
        context.CarrinhoClientes.Remove(carrinho);
        await context.SaveChangesAsync();
    }
    
    
}