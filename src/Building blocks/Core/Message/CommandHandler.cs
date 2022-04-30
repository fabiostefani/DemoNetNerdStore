﻿using Core.Data;
using FluentValidation.Results;

namespace Core.Message;

public abstract class CommandHandler
{
    protected ValidationResult ValidationResult;

    protected CommandHandler()
    {
        ValidationResult = new ValidationResult();
    }

    protected void AdicionarErro(string mensagem)
    {
        ValidationResult.Errors.Add(new ValidationFailure(String.Empty, mensagem));
    }

    protected async Task<ValidationResult> PersistirDados(IUnitOfWork uow)
    {
        if (!await uow.Commit())
            AdicionarErro("Houve um erro ao persistir os dados.");
        return ValidationResult;
    }
}