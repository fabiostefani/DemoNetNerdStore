namespace BFF.Compras.Models;

public class CarrinhoDto
{
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public List<ItemCarrinhoDto> Itens { get; set; } = new List<ItemCarrinhoDto>();

}