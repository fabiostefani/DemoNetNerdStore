using Core.Message;
using Pedidos.API.Application.Commands.Validations;
using Pedidos.API.Application.Dtos;

namespace Pedidos.API.Application.Commands;

public class AdicionarPedidoCommand : Command
{
    // Pedido
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public List<PedidoItemDto> PedidoItems { get; set; }

    // Voucher
    public string VoucherCodigo { get; set; }
    public bool VoucherUtilizado { get; set; }
    public decimal Desconto { get; set; }

    // Endereco
    public EnderecoDto Endereco { get; set; }

    // Cartao
    public string NumeroCartao { get; set; }
    public string NomeCartao { get; set; }
    public string ExpiracaoCartao { get; set; }
    public string CvvCartao { get; set; }

    public override bool EhValido()
    {
        ValidationResult = new AdicionarPedidoValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}