using Core.Data;

namespace Pedidos.Domain.Vouchers;

public interface IVoucherRepository : IRepository<Voucher>
{
    Task<Voucher?> ObterVoucherPorId(Guid id);
    Task<Voucher?> ObterVoucherPorCodigo(string codigo);
}