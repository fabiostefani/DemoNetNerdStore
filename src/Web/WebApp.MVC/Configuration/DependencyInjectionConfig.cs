using Api.Core.Extensions;
using Api.Core.Usuario;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Polly;
using WebApp.MVC.Extensions;
using WebApp.MVC.Services;
using WebApp.MVC.Services.Handlers;

namespace WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            
            
            services.AddTransient<HttpClientAuthorizationDelegationHandler>();
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(polly => polly.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())                
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(polly => polly.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            services.AddHttpClient<IComprasBffService, ComprasBffService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(polly => polly.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            
            services.AddHttpClient<IClienteService, ClienteService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(polly => polly.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            
            // services.AddHttpClient("Refit", options =>
            //     {
            //         options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            //     })
            //     .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
            //     .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);
            
            
        }
    }
}