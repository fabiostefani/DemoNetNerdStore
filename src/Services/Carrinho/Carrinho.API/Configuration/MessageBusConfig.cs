using Carrinho.API.Services;
using Core.Utils;
using MessageBus;

namespace Carrinho.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            .AddHostedService<CarrinhoIntegrationHandler>();
    }
}