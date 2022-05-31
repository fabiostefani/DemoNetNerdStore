namespace BFF.Compras.Models;

public class CarrinhoDto
{
    public decimal ValorTotal { get; set; }
    public VocuherDto Voucher { get; set; } = new VocuherDto();
    public bool VoucherUtilizado { get; set; }
    public decimal Desconto { get; set; }
    public List<ItemCarrinhoDto> Itens { get; set; } = new List<ItemCarrinhoDto>();

}