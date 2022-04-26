using WebApp.MVC.Extensions;
using WebApp.MVC.Services;
using WebApp.MVC.Services.Handlers;

namespace WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegationHandler>();
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
            
            // services.AddHttpClient<ICatalogoService, CatalogoService>()
            //     .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>();
            services.AddHttpClient("Refit", options =>
                {
                    options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
                })
                .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
                .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}