using Core.Message;
using FluentValidation.Results;
using MediatR;

namespace Pedidos.API.Application.Commands;

public class PedidoCommandoHandler : CommandHandler,
    IRequestHandler<AdicionarPedidoCommand, ValidationResult>
{
    public Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
    {
        return null;
    }
}