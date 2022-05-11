using Clientes.API.Services;
using Core.Utils;
using MessageBus;
using ConfigurationExtensions = Core.Utils.ConfigurationExtensions;

namespace Clientes.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMessageBus(configuration.GetMessageQueueConnection(ConfigurationExtensions.NomeConexao))
            .AddHostedService<RegistroClienteIntregrationHandler>();
    }
}