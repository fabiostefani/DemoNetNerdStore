using BFF.Compras.Extensions;
using BFF.Compras.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class PedidoService : IPedidoService
{
    private readonly HttpClient _httpClient;
    public PedidoService(HttpClient httpClient, 
                         IOptions<AppServicesSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.PedidorUrl) == false) 
            httpClient.BaseAddress = new Uri(settings.Value.PedidorUrl);
        _httpClient = httpClient;
    }
}