using System.Linq.Expressions;
using Core.Specification;

namespace Pedidos.Domain.Vouchers.Specs;

public class VoucherQuantidadeDisponivel : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression()
        => voucher => voucher.Quantidade > 0;
}