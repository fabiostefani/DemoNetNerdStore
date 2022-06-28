using System.Net;
using BFF.Compras.Extensions;
using BFF.Compras.Models;
using BFF.Compras.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class ClienteService : Service, IClienteService
{
    private readonly HttpClient _httpClient;

    public ClienteService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
    }

    public async Task<EnderecoDto> ObterEndereco()
    {
        var response = await _httpClient.GetAsync("/cliente/endereco/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<EnderecoDto>(response);
    }
}