using Api.Core.Usuario;
using Pagamentos.API.Data;
using Pagamentos.API.Data.Repository;
using Pagamentos.API.Facade;
using Pagamentos.API.Models;
using Pagamentos.API.Services;

namespace Pagamentos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<PagamentosContext>();
    }
}