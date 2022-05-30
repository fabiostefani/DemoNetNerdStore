using System.Net;
using BFF.Compras.Extensions;
using BFF.Compras.Models;
using BFF.Compras.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class PedidoService : Service, IPedidoService
{
    private readonly HttpClient _httpClient;
    public PedidoService(HttpClient httpClient, 
                         IOptions<AppServicesSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.PedidorUrl) == false) 
            httpClient.BaseAddress = new Uri(settings.Value.PedidorUrl);
        _httpClient = httpClient;
    }

    public async Task<VocuherDto?> ObterVocuherPorCodigo(string codigo)
    {
        var response = await _httpClient.GetAsync($"/voucher/{codigo}");
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        TratarErrosResponse(response);
        return await DeserializarObjetoResponse<VocuherDto>(response);

    }
}