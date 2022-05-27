namespace Pedidos.API.Application.Dtos;

public class VoucherDto
{
    public string Codigo { get; set; } = string.Empty;
    public decimal? Percentual { get; set; }
    public decimal? ValorDesconto { get; set; }
    public int TipoDesconto { get; set; }
}