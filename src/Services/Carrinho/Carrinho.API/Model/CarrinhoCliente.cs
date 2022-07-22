using Carrinho.API.Model.Validacoes;
using FluentValidation.Results;

namespace Carrinho.API.Model;

public partial class CarrinhoCliente
{
    internal const int MAX_QUANTIDADE_ITEM = 15;
    public Guid Id { get; private set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
    public ValidationResult ValidationResult { get; set; }
    public bool VoucherUtilizado { get; set; }
    public decimal Desconto { get; set; }
    public Voucher? Voucher { get; set; }

    public CarrinhoCliente(Guid clienteId)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
    }
    public CarrinhoCliente() { }

    internal void AdicionarItem(CarrinhoItem item)
    {
        item.AssociarItem(Id);
        if (CarrinhoItemExistente(item))
        {
            CarrinhoItem itemExistente = ObterPorProdutoId(item.ProdutoId);
            itemExistente.AdicionarUnidades(item.Quantidade);
            item = itemExistente;
            Itens.Remove((itemExistente));
        }
        Itens.Add(item);
        CalcularValorCarrinho();
    }

    internal void AtualizarItem(CarrinhoItem item)
    {
        item.AssociarCarrinho(item.CarrinhoId.Value);
        CarrinhoItem itemExistente = ObterPorProdutoId(item.ProdutoId);
        Itens.Remove(itemExistente);
        Itens.Add(item);
        CalcularValorCarrinho();
    }

    internal void RemoverItem(CarrinhoItem item)
    {
        CarrinhoItem itemExistente = ObterPorProdutoId(item.ProdutoId);
        Itens.Remove(itemExistente);
        CalcularValorCarrinho();
    }

    internal void AtualizarUnidades(CarrinhoItem item, int unidades)
    {
        item.AtualizarUnidades(unidades);
        AtualizarItem(item);
    }

    internal bool EhValido()
    {
        List<ValidationFailure> erros = Itens.SelectMany(i => new ItemCarrinhoValidation().Validate(i).Errors).ToList();
        erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);
        ValidationResult = new ValidationResult(erros);
        return ValidationResult.IsValid;
    }

    public void AplicarVoucher(Voucher voucher)
    {
        Voucher = voucher;
        VoucherUtilizado = true;
        CalcularValorCarrinho();
    }

    private void CalcularValorTotalDesconto()
    {
        if (!VoucherUtilizado) return;
        decimal desconto = 0;
        var valor = ValorTotal;
        if (Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
        {
            if (Voucher.Percentual.HasValue)
            {
                desconto = (valor * Voucher.Percentual.Value) / 100;
                valor -= desconto;
            }
        }
        else
        {
            if (Voucher.ValorDesconto.HasValue)
            {
                desconto = Voucher.ValorDesconto.Value;
                valor -= desconto;
            }
        }
        ValorTotal = valor < 0 ? 0 : valor;
        Desconto = desconto;
    }

    internal void CalcularValorCarrinho()
    {
        ValorTotal = Itens.Sum(i => i.CalcularValor());
        CalcularValorTotalDesconto();
    }

    internal bool CarrinhoItemExistente(CarrinhoItem item)
        => Itens.Any(i => i.ProdutoId == item.ProdutoId);

    internal CarrinhoItem ObterPorProdutoId(Guid produtoId)
        => Itens.FirstOrDefault(p=>p.ProdutoId == produtoId);
}