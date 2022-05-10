using Clientes.API.Application.Commands;
using Core.Mediator;
using Core.Message.Integration;
using EasyNetQ;
using FluentValidation.Results;

namespace Clientes.API.Services;

public class RegistroClienteIntregrationHandler : BackgroundService
{
    private IBus? _bus;
    private readonly IServiceProvider _serviceProvider;
    public RegistroClienteIntregrationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus = RabbitHutch.CreateBus("host=localhost:5672");
        _bus.Rpc.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
            new ResponseMessage(await RegistrarClient(request))
        );
        return Task.CompletedTask;
    }

    private async Task<ValidationResult> RegistrarClient(UsuarioRegistradoIntegrationEvent message)
    {
        var registrarClienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        ValidationResult sucesso = await mediator.EnviarComando(registrarClienteCommand);
        return sucesso;
    }
}