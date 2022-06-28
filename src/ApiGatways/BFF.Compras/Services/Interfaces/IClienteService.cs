using BFF.Compras.Models;

namespace BFF.Compras.Services.Interfaces;

public interface IClienteService
{
    Task<EnderecoDto> ObterEndereco();
}