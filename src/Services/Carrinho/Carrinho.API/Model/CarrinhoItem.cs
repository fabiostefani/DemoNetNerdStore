namespace Carrinho.API.Model;

public class CarrinhoItem
{
    public Guid Id { get; private set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; }= string.Empty;
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
    public string Imagem { get; set; } = string.Empty;
    public Guid CarrinhoId { get; set; }
    public CarrinhoCliente CarrinhoCliente { get; set; }
    
    public CarrinhoItem()
    {
        Id = Guid.NewGuid();
    }
}