namespace Core.Message.Integration;

public class PedidoPagoIntegrationEvent : IntegrationEvent
{
    public PedidoPagoIntegrationEvent(Guid clienteId, Guid pedidoId)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
    }

    public Guid PedidoId { get; private set; }
    public Guid ClienteId { get; private set; }
}