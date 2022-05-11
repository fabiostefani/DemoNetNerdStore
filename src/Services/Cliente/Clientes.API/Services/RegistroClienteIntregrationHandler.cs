using Clientes.API.Application.Commands;
using Core.Mediator;
using Core.Message.Integration;
using FluentValidation.Results;
using MessageBus;

namespace Clientes.API.Services;

public class RegistroClienteIntregrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;
    public RegistroClienteIntregrationHandler(IServiceProvider serviceProvider, 
                                              IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
            await RegistrarClient(request)
        );
        return Task.CompletedTask;
    }

    private async Task<ResponseMessage> RegistrarClient(UsuarioRegistradoIntegrationEvent message)
    {
        var registrarClienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        ValidationResult sucesso = await mediator.EnviarComando(registrarClienteCommand);
        return new ResponseMessage(sucesso);
    }
}