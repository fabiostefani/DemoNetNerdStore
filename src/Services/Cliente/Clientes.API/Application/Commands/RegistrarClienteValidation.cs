using Core.DomainObjects;
using FluentValidation;

namespace Clientes.API.Application.Commands;

public class RegistrarClienteValidation : AbstractValidator<RegistrarClienteCommand>
{
    public RegistrarClienteValidation()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("ID do cliente inválido");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome do cliente não foi informado.");

        RuleFor(x => x.Cpf)
            .Must(TerCpfValido)
            .WithMessage("O CPF informado não é válido");

        RuleFor(x => x.Email)
            .Must(TerEmailValido)
            .WithMessage("O e-mail informado não é válido.");
    }

    private static bool TerCpfValido(string cpf)
        => Cpf.Validar(cpf);

    private static bool TerEmailValido(string email)
        => Email.Validar(email);
}