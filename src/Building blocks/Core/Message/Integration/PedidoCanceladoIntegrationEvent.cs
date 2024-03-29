﻿namespace Core.Message.Integration;

public class PedidoCanceladoIntegrationEvent : IntegrationEvent
{
    public PedidoCanceladoIntegrationEvent(Guid clienteId, Guid pedidoId)
    {
        ClienteId = clienteId;
        PedidoId = pedidoId;
    }

    public Guid ClienteId { get; private set; }
    public Guid PedidoId { get; private set; }
}