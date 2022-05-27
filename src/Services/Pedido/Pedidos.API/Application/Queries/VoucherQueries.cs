using Pedidos.API.Application.Dtos;
using Pedidos.Domain.Vouchers;

namespace Pedidos.API.Application.Queries;

public class VoucherQueries : IVoucherQueries
{
    private readonly IVoucherRepository _voucherRepository;
    public VoucherQueries(IVoucherRepository voucherRepository)
    {
        _voucherRepository = voucherRepository;
    }
    public async Task<VoucherDto?> ObterVoucherPorCodigo(string codigo)
    {
        var voucher = await _voucherRepository.ObterVoucherPorCodigo(codigo);
        if (voucher == null) return null;
        return new VoucherDto()
        {
            Codigo = voucher.Codigo,
            TipoDesconto = (int) voucher.TipoDesconto,
            Percentual = voucher.Percentual,
            ValorDesconto = voucher.ValorDesconto
        };
    }
}