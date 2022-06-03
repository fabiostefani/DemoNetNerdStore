using Pedidos.Domain.Pedidos;

namespace Pedidos.API.Application.Dtos;

public class PedidoItemDto
{
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Imagem { get; set; } = string.Empty;
    public int Quantidade { get; set; }

    public static PedidoItem ParaPedidoItem(PedidoItemDto pedidoItemDTO)
    {
        return new PedidoItem(pedidoItemDTO.ProdutoId, pedidoItemDTO.Nome, pedidoItemDTO.Quantidade,
            pedidoItemDTO.Valor, pedidoItemDTO.Imagem);
    }
}