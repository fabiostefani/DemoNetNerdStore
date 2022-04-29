using Clientes.API.Data;

namespace Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ClientesContext>();
        return services;
    }
}