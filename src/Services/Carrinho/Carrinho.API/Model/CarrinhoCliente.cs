namespace Carrinho.API.Model;

public class CarrinhoCliente
{
    internal const int MAX_QUANTIDADE_ITEM = 15;
    public Guid Id { get; private set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();

    public CarrinhoCliente(Guid clienteId)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
    }
    public CarrinhoCliente() { }

    internal void AdicionarItem(CarrinhoItem item)
    {
        if (!item.EhValido())
            return;

        item.AssociarItem(Id);
        if (CarrinhoItemExistente(item))
        {
            CarrinhoItem itemExistente = ObterPorProdutoId(item.ProdutoId);
            itemExistente.AdicionarUnidades(item.Quantidade);
            item = itemExistente;
            Itens.Remove((itemExistente));
        }
        Itens.Add(item);
        CalcularValorTotalCarrinho();
    }

    internal void CalcularValorTotalCarrinho()
        => ValorTotal = Itens.Sum(i => i.CalcularValor());

    internal bool CarrinhoItemExistente(CarrinhoItem item)
        => Itens.Any(i => i.ProdutoId == item.ProdutoId);
    
    internal CarrinhoItem ObterPorProdutoId(Guid produtoId)
        => Itens.FirstOrDefault(p=>p.ProdutoId == produtoId);
}