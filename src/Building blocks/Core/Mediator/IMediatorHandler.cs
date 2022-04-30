using Core.Message;
using FluentValidation.Results;

namespace Core.Mediator;

public interface IMediatorHandler
{
    Task PublicarEvento<T>(T evento) where T : Event;
    Task<ValidationResult> EnviarComando<T>(T commando) where T : Command;
}