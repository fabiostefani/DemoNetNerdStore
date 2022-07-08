using Pedidos.Domain.Pedidos;

namespace Pedidos.API.Application.Dtos;

public class PedidoDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public Guid ClienteId { get; set; }
    public int Status { get; set; }
    public DateTime Data { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public string VoucherCodigo { get; set; } = string.Empty;
    public bool VoucherUtilizado { get; set; }
    public List<PedidoItemDto> PedidoItens { get; set; } = new List<PedidoItemDto>();
    public EnderecoDto Endereco { get; set; } = new EnderecoDto();

    public static PedidoDto ParaPedidoDto(Pedido pedido)
    {
        var pedidoDto = new PedidoDto
        {
            Id = pedido.Id,
            Codigo = pedido.Codigo,
            Status = (int)pedido.PedidoStatus,
            Data = pedido.DataCadastro,
            ValorTotal = pedido.ValorTotal,
            Desconto = pedido.Desconto,
            VoucherUtilizado = pedido.VoucherUtilizado,
            PedidoItens = new List<PedidoItemDto>(),
            Endereco = new EnderecoDto()
        };

        foreach (var item in pedido.PedidoItens)
        {
            pedidoDto.PedidoItens.Add(new PedidoItemDto
            {
                Nome = item.ProdutoNome,
                Imagem = item.ProdutoImagem,
                Quantidade = item.Quantidade,
                ProdutoId = item.ProdutoId,
                Valor = item.ValorUnitario,
                PedidoId = item.PedidoId
            });
        }

        pedidoDto.Endereco = new EnderecoDto
        {
            Logradouro = pedido.Endereco.Logradouro,
            Numero = pedido.Endereco.Numero,
            Complemento = pedido.Endereco.Complemento,
            Bairro = pedido.Endereco.Bairro,
            Cep = pedido.Endereco.Cep,
            Cidade = pedido.Endereco.Cidade,
            Estado = pedido.Endereco.Estado,
        };

        return pedidoDto;
    }
}