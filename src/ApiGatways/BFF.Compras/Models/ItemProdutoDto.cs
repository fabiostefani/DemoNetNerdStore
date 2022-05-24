namespace BFF.Compras.Models;

public class ItemProdutoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Imagem { get; set; } = string.Empty;
    public int QuantidadeEstoque { get; set; }
}