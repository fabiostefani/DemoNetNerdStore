using Api.Core.Controllers;
using Api.Core.Usuario;
using Core.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pedidos.API.Application.Commands;
using Pedidos.API.Application.Queries;

namespace Pedidos.API.Controllers;

[Authorize]
public class PedidoController : MainController
{
    private readonly IAspNetUser _user;
    private readonly IPedidoQueries _pedidoQueries;
    private readonly IMediatorHandler _mediator;

    public PedidoController(IMediatorHandler mediator,
                            IAspNetUser user,
                            IPedidoQueries pedidoQueries)
    {
        _user = user;
        _pedidoQueries = pedidoQueries;
        _mediator = mediator;
    }

    [HttpPost("pedido")]
    public async Task<IActionResult> AdicionarPedido(AdicionarPedidoCommand pedido)
    {
        pedido.ClienteId = _user.ObterUserId();
        return CustomResponse(await _mediator.EnviarComando(pedido));
    }

    [HttpGet("pedido/ultimo")]
    public async Task<IActionResult> UltimoPedido()
    {
        var pedido = await _pedidoQueries.ObterUltimoPedido(_user.ObterUserId());

        return pedido is null ? NotFound() : CustomResponse(pedido);
    }

    [HttpGet("pedido/lista-cliente")]
    public async Task<IActionResult> ListaPorCliente()
    {
        var pedidos = await _pedidoQueries.ObterListaPorClienteId(_user.ObterUserId());

        return pedidos == null ? NotFound() : CustomResponse(pedidos);
    }
}