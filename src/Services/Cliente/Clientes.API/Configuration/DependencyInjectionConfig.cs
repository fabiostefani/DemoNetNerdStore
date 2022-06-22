using Clientes.API.Application.Commands;
using Clientes.API.Application.Events;
using Clientes.API.Data;
using Clientes.API.Data.Repository;
using Clientes.API.Models;
using Clientes.API.Services;
using Core.Mediator;
using FluentValidation.Results;
using MediatR;

namespace Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
        services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();
        services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ClientesContext>();
        return services;
    }
}