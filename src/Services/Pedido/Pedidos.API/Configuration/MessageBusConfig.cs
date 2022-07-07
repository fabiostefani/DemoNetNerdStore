using Core.Utils;
using MessageBus;
using Pedidos.API.Servicees;

namespace Pedidos.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            .AddHostedService<PedidoOrquestradorIntegrationHandler>();
    }
}