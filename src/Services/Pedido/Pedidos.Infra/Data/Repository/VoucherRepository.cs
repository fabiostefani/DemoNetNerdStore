using Core.Data;
using Pedidos.Domain.Vouchers;

namespace Pedidos.Infra.Data.Repository;

public class VoucherRepository : IVoucherRepository
{
    private readonly PedidosContext _context;
    public VoucherRepository(PedidosContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}