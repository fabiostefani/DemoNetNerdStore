using Api.Core.Extensions;
using Api.Core.Usuario;
using BFF.Compras.Extensions;
using BFF.Compras.Services;
using BFF.Compras.Services.Interfaces;
using Polly;

namespace BFF.Compras.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServicecs(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddTransient<HttpClientAuthorizationDelegationHandler>();
        services.AddHttpClient<ICatalogoService, CatalogoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
            .AddPolicyHandler(PollyExtensions.EsperarTentar())
            .AllowSelfSignedCertificate()
            .AddTransientHttpErrorPolicy(polly => polly.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        services.AddHttpClient<ICarrinhoService, CarrinhoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
            .AddPolicyHandler(PollyExtensions.EsperarTentar())
            .AllowSelfSignedCertificate()
            .AddTransientHttpErrorPolicy(polly => polly.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        services.AddHttpClient<IPedidoService, PedidoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
            .AddPolicyHandler(PollyExtensions.EsperarTentar())
            .AllowSelfSignedCertificate()
            .AddTransientHttpErrorPolicy(polly => polly.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        services.AddHttpClient<IClienteService, ClienteService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
            .AddPolicyHandler(PollyExtensions.EsperarTentar())
            .AllowSelfSignedCertificate()
            .AddTransientHttpErrorPolicy(polly => polly.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

    }
}