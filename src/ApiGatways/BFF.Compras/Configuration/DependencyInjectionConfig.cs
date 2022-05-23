using Api.Core.Usuario;

namespace BFF.Compras.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServicecs(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
    }
}