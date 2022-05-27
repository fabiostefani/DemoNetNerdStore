using Api.Core.Usuario;
using Core.Mediator;
using Pedidos.Domain.Vouchers;
using Pedidos.Infra.Data;
using Pedidos.Infra.Data.Repository;

namespace Pedidos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<PedidosContext>();
        
    }
}