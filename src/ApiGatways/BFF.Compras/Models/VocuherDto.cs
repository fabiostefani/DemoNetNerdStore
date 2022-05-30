namespace BFF.Compras.Models;

public class VocuherDto
{
    public decimal? Percentual { get; set; }
    public decimal? ValorDesconto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public int TipoDesconto { get; set; }
}