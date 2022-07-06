using Core.Utils;
using MessageBus;
using Pagamentos.API.Services;

namespace Pagamentos.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            .AddHostedService<PagamentoIntegrationHandler>();
    }
}