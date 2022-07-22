using Api.Core.Usuario;
using Carrinho.API.Data;
using Carrinho.API.Model;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Carrinho.API.Services.gRPC;

[Authorize]
public class CarrinhoGrpcService : CarrinhoCompras.CarrinhoComprasBase
{
    private readonly ILogger<CarrinhoGrpcService> _loger;
    private readonly IAspNetUser _user;
    private readonly CarrinhoContext _context;

    public CarrinhoGrpcService(ILogger<CarrinhoGrpcService> loger, 
                               IAspNetUser user, 
                               CarrinhoContext context)
    {
        _loger = loger;
        _user = user;
        _context = context;
    }

    public override async Task<CarrinhoClienteResponse> ObterCarrinho(ObterCarrinhoRequest request, ServerCallContext context)
    {
        _loger.LogInformation("Chamando ObterCarrinho");
        var carrinho = await ObterCarrinhoCliente();
        return MapCarrinhoClienteToProtoResponde(carrinho);
    }

    private async Task<CarrinhoCliente> ObterCarrinhoCliente()
    {
        var clienteId = _user.ObterUserId();
        return await _context.CarrinhoClientes
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId) ?? new CarrinhoCliente(clienteId);
    }

    private static CarrinhoClienteResponse MapCarrinhoClienteToProtoResponde(CarrinhoCliente carrinho)
    {
        var carrinhoProto = new CarrinhoClienteResponse
        {
            Id = carrinho.Id.ToString(),
            Clienteid = carrinho.ClienteId.ToString(),
            Valortotal = (double) carrinho.ValorTotal,
            Desconto = (double) carrinho.Desconto,
            Voucherutillizado = carrinho.VoucherUtilizado
        };
        if (carrinho.Voucher != null)
        {
            carrinhoProto.Voucher = new VoucherResponse
            {
                Codigo = carrinho.Voucher.Codigo,
                Percentual = (double?) carrinho.Voucher.Percentual ?? 0,
                Tipodesconto = (int) carrinho.Voucher.TipoDesconto,
                Valordesconto = (double?) carrinho.Voucher.ValorDesconto ?? 0
            };
        }

        foreach (var item in carrinho.Itens)
        {
            carrinhoProto.Itens.Add(new CarrinhoItemResponse
            {
                Id = item.Id.ToString(),
                Nome = item.Nome,
                Imagem = item.Imagem,
                Produtoid = item.ProdutoId.ToString(),
                Quantidade = item.Quantidade,
                Valor = (double)item.Valor
            });
        }

        return carrinhoProto;
    }
}