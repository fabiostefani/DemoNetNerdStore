using BFF.Compras.Extensions;
using BFF.Compras.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class CatalogoService : ICatalogoService
{
    private readonly HttpClient _httpClient;
    public CatalogoService(HttpClient httpClient,
                           IOptions<AppServicesSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.CatalogoUrl) == false)
            httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        _httpClient = httpClient;
    }
}