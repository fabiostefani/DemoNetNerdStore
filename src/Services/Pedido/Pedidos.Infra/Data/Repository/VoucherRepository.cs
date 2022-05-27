using Core.Data;
using Microsoft.EntityFrameworkCore;
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

    public async Task<Voucher?> ObterVoucherPorId(Guid id)
        => await _context.Vouchers.FirstOrDefaultAsync(v => v.Id == id);
    
    public async Task<Voucher?> ObterVoucherPorCodigo(string codigo)
        => await _context.Vouchers.FirstOrDefaultAsync(v => Equals(v.Codigo, codigo));

    public void Dispose()
        => _context.Dispose();

}