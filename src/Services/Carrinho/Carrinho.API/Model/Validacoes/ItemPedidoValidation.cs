using FluentValidation;

namespace Carrinho.API.Model.Validacoes;

public class ItemPedidoValidation : AbstractValidator<CarrinhoItem>
{
    public ItemPedidoValidation()
    {
        RuleFor(c => c.ProdutoId)
            .NotEqual(Guid.Empty)
            .WithMessage("ID do produto inválido");
        
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage("O Nome do produto não foi informado.");

        RuleFor(c => c.Quantidade)
            .GreaterThan(0)
            .WithMessage("A quantidade mínima de um item é 1");
        
        RuleFor(c => c.Quantidade)
            .LessThan(CarrinhoCliente.MAX_QUANTIDADE_ITEM)
            .WithMessage($"A quantidade máxima de um item é {CarrinhoCliente.MAX_QUANTIDADE_ITEM}");
        
        RuleFor(c => c.Valor)
            .GreaterThan(0)
            .WithMessage("O valor do item precisa ser maior que 0");
    }
}