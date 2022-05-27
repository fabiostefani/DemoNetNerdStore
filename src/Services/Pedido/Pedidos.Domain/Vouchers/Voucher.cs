using Core.DomainObjects;
using Pedidos.Domain.Vouchers.Specs;

namespace Pedidos.Domain.Vouchers;

public class Voucher : Entity, IAggregateRoot
{
    public string Codigo { get; private set; } = string.Empty;
    public decimal? Percentual { get; private set; }
    public decimal? ValorDesconto { get; private set; }
    public int Quantidade { get; private set; }
    public TipoDescontoVoucher TipoDesconto { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataUtilizacao { get; private set; }
    public DateTime DataValidade { get; private set; }
    public bool Ativo { get; private set; }
    public bool Utilizado { get; private set; }

    public bool EstaValidoParaUtilizacao()
        => new VoucherAtivoSpecification()
            .And(new VoucherDataSpecification())
            .And(new VoucherQuantidadeDisponivel())
            .IsSatisfiedBy(this);
    

    public void MarcarComoUtilizado()
    {
        Ativo = false;
        Utilizado = true;
        Quantidade = 0;
    }
}