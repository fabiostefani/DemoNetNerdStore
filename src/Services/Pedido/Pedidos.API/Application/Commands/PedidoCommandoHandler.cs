using System.Reflection.Metadata.Ecma335;
using Core.Message;
using Core.Message.Integration;
using FluentValidation.Results;
using MediatR;
using MessageBus;
using Pedidos.API.Application.Dtos;
using Pedidos.API.Application.Events;
using Pedidos.Domain.Pedidos;
using Pedidos.Domain.Vouchers;
using Pedidos.Domain.Vouchers.Specs;

namespace Pedidos.API.Application.Commands;

public class PedidoCommandoHandler : CommandHandler,
    IRequestHandler<AdicionarPedidoCommand, ValidationResult>
{
    private readonly IVoucherRepository _voucherRepository;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IMessageBus _messageBus;

    public PedidoCommandoHandler(IVoucherRepository voucherRepository,
                                 IPedidoRepository pedidoRepository,
                                 IMessageBus messageBus)
    {
        _voucherRepository = voucherRepository;
        _pedidoRepository = pedidoRepository;
        _messageBus = messageBus;
    }
    public async Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
    {
        if (message.EhValido()) return message.ValidationResult;
        var pedido = MapearPedido(message);
        if (!await AplicarVoucher(message, pedido)) return ValidationResult;
        if (!ValidarPedido(pedido)) return ValidationResult;
        if (!await ProcessarPagamento(pedido, message)) return ValidationResult;
        pedido.AutorizarPedido();
        pedido.AdicionarEvento(new PedidoRealizadoEvent(pedido.Id, pedido.ClienteId));
        _pedidoRepository.Adicionar(pedido);
        return await PersistirDados(_pedidoRepository.UnitOfWork);
    }

    private Pedido MapearPedido(AdicionarPedidoCommand message)
    {
        var endereco = new Endereco
        {
            Logradouro = message.Endereco.Logradouro,
            Numero = message.Endereco.Numero,
            Bairro = message.Endereco.Bairro,
            Cep = message.Endereco.Cep,
            Cidade = message.Endereco.Cidade,
            Complemento = message.Endereco.Complemento,
            Estado = message.Endereco.Estado
        };
        var pedido = new Pedido(message.ClienteId, message.ValorTotal,
            message.PedidoItems.Select(PedidoItemDto.ParaPedidoItem).ToList(), message.VoucherUtilizado,
            message.Desconto);
        pedido.AtribuirEndereco(endereco);
        return pedido;
    }

    private async Task<bool> AplicarVoucher(AdicionarPedidoCommand message, Pedido pedido)
    {
        if (!message.VoucherUtilizado) return true;
        var voucher = await _voucherRepository.ObterVoucherPorCodigo(message.VoucherCodigo);
        if (voucher is null)
        {
            AdicionarErro("O Voucher informado não existe.");
            return false;
        }
        var voucherValidation = new VoucherValidation().Validate(voucher);
        if (!voucherValidation.IsValid)
        {
            voucherValidation.Errors.ToList().ForEach(me => AdicionarErro(me.ErrorMessage));
            return false;
        }
        pedido.AtribuirVoucher(voucher);
        voucher.DebitarQuantidade();
        _voucherRepository.Atualizar(voucher);
        return true;
    }

    private bool ValidarPedido(Pedido pedido)
    {
        var pedidoValorOriginal = pedido.ValorTotal;
        var pedidoDesconto = pedido.Desconto;
        pedido.CalcularValorPedido();
        if (pedido.ValorTotal != pedidoValorOriginal)
        {
            AdicionarErro("O Valor total do pedido não confere com o cálculo do pedido.");
            return false;
        }
        if (pedido.Desconto != pedidoDesconto)
        {
            AdicionarErro("O valor total não confere com o cálculo do pedido.");
            return false;
        }
        return true;
    }

    private async Task<bool> ProcessarPagamento(Pedido pedido, AdicionarPedidoCommand message)
    {
        var pedidoIniciado = new PedidoIniciadoIntegrationEvent
        {
            PedidoId = pedido.Id,
            ClienteId = pedido.ClienteId,
            Valor = pedido.ValorTotal,
            TipoPagamento = 1,
            NomeCartao = message.NomeCartao,
            Cvv = message.CvvCartao,
            NumeroCartao = message.NumeroCartao,
            MesAnoVencimento = message.ExpiracaoCartao
        };
        var result = await _messageBus.RequestAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(pedidoIniciado);

        if (result.ValidationResult.IsValid) return true;

        foreach (var error in result.ValidationResult.Errors)
        {
            AdicionarErro(error.ErrorMessage);
        }
        
        return false;
    }
        
}