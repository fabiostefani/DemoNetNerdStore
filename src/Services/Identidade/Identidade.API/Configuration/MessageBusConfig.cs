using MessageBus;
using Core.Utils;
using ConfigurationExtensions = Core.Utils.ConfigurationExtensions;

namespace Identidade.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection(ConfigurationExtensions.NomeConexao));
    }
}