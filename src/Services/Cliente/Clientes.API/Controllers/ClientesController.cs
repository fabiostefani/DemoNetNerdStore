using Api.Core.Controllers;
using Clientes.API.Application.Commands;
using Core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Clientes.API.Controllers;

public class ClientesController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;

    public ClientesController(IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }
    [HttpGet("clientes")]
    public async Task<IActionResult> Index()
    {
       var resultado = await _mediatorHandler.EnviarComando(new RegistrarClienteCommand(Guid.NewGuid(), "Fabio", "fabiostefani@gmail.com",
            "03749377960"));
       return CustomResponse(resultado);
    }
}