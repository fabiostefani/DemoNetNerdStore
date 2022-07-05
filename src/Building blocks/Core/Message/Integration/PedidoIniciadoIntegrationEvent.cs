namespace Core.Message.Integration;

public class PedidoIniciadoIntegrationEvent : IntegrationEvent
{
    public Guid ClienteId { get; set; }
    public Guid PedidoId { get; set; }
    public int TipoPagamento { get; set; }
    public decimal Valor { get; set; }
    public string NomeCartao { get; set; } = string.Empty;
    public string NumeroCartao { get; set; } = string.Empty;
    public string MesAnoVencimento { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
}