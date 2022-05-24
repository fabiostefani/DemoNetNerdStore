namespace BFF.Compras.Models;

public class ItemCarrinhoDto
{
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Imagem { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}