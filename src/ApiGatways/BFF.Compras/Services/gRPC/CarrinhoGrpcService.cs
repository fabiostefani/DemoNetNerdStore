using BFF.Compras.Models;
using Carrinho.API.Services.gRPC;

namespace BFF.Compras.Services.gRPC;

public class CarrinhoGrpcService : ICarrinhoGrpcService
{
    private readonly CarrinhoCompras.CarrinhoComprasClient _carrinhoComprasClient;

    public CarrinhoGrpcService(CarrinhoCompras.CarrinhoComprasClient carrinhoComprasClient)
    {
        _carrinhoComprasClient = carrinhoComprasClient;
    }

    public async Task<CarrinhoDto> ObterCarrinho()
    {
        var response = await _carrinhoComprasClient.ObterCarrinhoAsync(new ObterCarrinhoRequest());
        return MapCarrinhoClienteProtoResponseToDto(response);
    }

    private static CarrinhoDto MapCarrinhoClienteProtoResponseToDto(CarrinhoClienteResponse carrinhoResponse)
    {
        var carrinhoDto = new CarrinhoDto
        {
            ValorTotal = (decimal) carrinhoResponse.Valortotal,
            Desconto = (decimal) carrinhoResponse.Desconto,
            VoucherUtilizado = carrinhoResponse.Voucherutillizado
        };
        MapVoucher(carrinhoResponse, carrinhoDto);
        foreach (var item in carrinhoResponse.Itens)
        {
            carrinhoDto.Itens.Add(MapCarrinhoItem(item));
        }
        return carrinhoDto;
    }

    private static void MapVoucher(CarrinhoClienteResponse carrinhoResponse, CarrinhoDto carrinhoDto)
    {
        if (carrinhoResponse.Voucher == null) return;
        carrinhoDto.Voucher = new VocuherDto
        {
            Codigo = carrinhoResponse.Voucher.Codigo,
            Percentual = (decimal?) carrinhoResponse.Voucher.Percentual,
            TipoDesconto = carrinhoResponse.Voucher.Tipodesconto,
            ValorDesconto = (decimal?) carrinhoResponse.Voucher.Valordesconto
        };
    }

    private static ItemCarrinhoDto MapCarrinhoItem(CarrinhoItemResponse item)
    {
        return new ItemCarrinhoDto
        {
            Imagem = item.Imagem,
            Nome = item.Nome,
            Quantidade = item.Quantidade,
            Valor = (decimal) item.Valor,
            ProdutoId = Guid.Parse(item.Produtoid)
        };
    }
}