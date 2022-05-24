using BFF.Compras.Extensions;
using BFF.Compras.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class CarrinhoService : ICarrinhoService
{
    private readonly HttpClient _httpClient;
    public CarrinhoService(HttpClient httpClient, 
                           IOptions<AppServicesSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.CarrinhoUrl) == false) 
            httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        _httpClient = httpClient;
    }
}