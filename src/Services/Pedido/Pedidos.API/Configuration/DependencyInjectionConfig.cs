using Api.Core.Usuario;
using Core.Mediator;
using FluentValidation.Results;
using MediatR;
using Pedidos.API.Application.Commands;
using Pedidos.API.Application.Events;
using Pedidos.API.Application.Queries;
using Pedidos.Domain.Pedidos;
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
        services.AddScoped<IVoucherQueries, VoucherQueries>();
        services.AddScoped<IPedidoQueries, PedidoQueries>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<PedidosContext>();

        services.AddScoped<IRequestHandler<AdicionarPedidoCommand, ValidationResult>, PedidoCommandoHandler>();
        services.AddScoped<INotificationHandler<PedidoRealizadoEvent>, PedidoEventHandler>();

    }
}