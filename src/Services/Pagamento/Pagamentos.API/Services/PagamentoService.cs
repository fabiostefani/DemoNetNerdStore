﻿using Core.Message.Integration;
using FluentValidation.Results;
using Pagamentos.API.Facade;
using Pagamentos.API.Models;

namespace Pagamentos.API.Services;

public class PagamentoService : IPagamentoService
{
    private readonly IPagamentoFacade _pagamentoFacade;
    private readonly IPagamentoRepository _pagamentoRepository;
        
    public PagamentoService(IPagamentoFacade pagamentoFacade, 
                           IPagamentoRepository pagamentoRepository)
    {
        _pagamentoFacade = pagamentoFacade;
        _pagamentoRepository = pagamentoRepository;
    }

    public async Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento)
    {
        var transacao = await _pagamentoFacade.AutorizarPagamento(pagamento);
        var validationResult = new ValidationResult();
        if (transacao.Status != StatusTransacao.Autorizado)
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", "Pagamento recusado, entre em contato com a sua operadora de cartão."));
            return new ResponseMessage(validationResult);
        }
        pagamento.AdicionarTransacao(transacao);
        _pagamentoRepository.AdicionarPagamento(pagamento);
        if (!await _pagamentoRepository.UnitOfWork.Commit())
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", "Houve um erro ao realizar o pagamento."));
            //_pagamentoFacade.EstornarPagamento(transacao.NSU);
            return new ResponseMessage(validationResult);
        }
        return new ResponseMessage(validationResult);
    }
}