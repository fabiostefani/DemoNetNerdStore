using BFF.Compras.Extensions;
using BFF.Compras.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class PagamentoService : IPagamentoService
{
    private readonly HttpClient _httpClient;
    public PagamentoService(HttpClient httpClient, 
                            IOptions<AppServicesSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.PagamentoUrl) == false) 
            httpClient.BaseAddress = new Uri(settings.Value.PagamentoUrl);
        _httpClient = httpClient;
    }
}