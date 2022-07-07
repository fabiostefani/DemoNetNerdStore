using Pedidos.API.Application.Queries;

namespace Pedidos.API.Servicees;

public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PedidoOrquestradorIntegrationHandler> _logger;
    private Timer _timer = null!;

    public PedidoOrquestradorIntegrationHandler(ILogger<PedidoOrquestradorIntegrationHandler> logger, 
                                                IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Serviço de pedido iniciado");
        _timer = new Timer(ProcessarPedidos, null, TimeSpan.Zero, TimeSpan.FromSeconds(15);
        return Task.CompletedTask;
    }

    private async void ProcessarPedidos(object? state)
    {
        _logger.LogInformation("Processando pedidos");
        using (var scope = _serviceProvider.CreateScope())
        {
            var pedidoQueries = scope.ServiceProvider.GetRequiredService<IPedidoQueries>();
            var pedido = await pedidoQueries.ObterPedidosAutorizados();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Serviço de pedidos finalizado");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}