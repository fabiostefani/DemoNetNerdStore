using Api.Core.Usuario;
using Carrinho.API.Data;

namespace Carrinho.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<CarrinhoContext>();
        return services;
    }
}