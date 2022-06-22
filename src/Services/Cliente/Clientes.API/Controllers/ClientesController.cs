using Api.Core.Controllers;
using Api.Core.Usuario;
using Clientes.API.Application.Commands;
using Clientes.API.Models;
using Core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Clientes.API.Controllers;

public class ClientesController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IClienteRepository _clienteRepository;
    private readonly IAspNetUser _user;

    public ClientesController(IMediatorHandler mediatorHandler,
                              IClienteRepository clienteRepository,
                              IAspNetUser user)
    {
        _mediatorHandler = mediatorHandler;
        _clienteRepository = clienteRepository;
        _user = user;
    }
    // [HttpGet("clientes")]
    // public async Task<IActionResult> Index()
    // {
    //    var resultado = await _mediatorHandler.EnviarComando(new RegistrarClienteCommand(Guid.NewGuid(), "Fabio", "fabiostefani@gmail.com",
    //         "03749377960"));
    //    return CustomResponse(resultado);
    // }

    [HttpGet("cliente/endereco")]
    public async Task<IActionResult> ObterEndereco()
    {
        var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUserId());
        return endereco is null ? NotFound() : CustomResponse(endereco);
    }

    [HttpPost("cliente/endereco")]
    public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
    {
        endereco.ClienteId = _user.ObterUserId();
        return CustomResponse(await _mediatorHandler.EnviarComando(endereco));
    }
}