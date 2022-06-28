using BFF.Compras.Extensions;
using BFF.Compras.Models;
using BFF.Compras.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class CatalogoService : Service, ICatalogoService
{
    private readonly HttpClient _httpClient;
    public CatalogoService(HttpClient httpClient,
                           IOptions<AppServicesSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.CatalogoUrl) == false)
            httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        _httpClient = httpClient;
    }

    public async Task<ItemProdutoDto> ObterPorId(Guid id)
    {
        var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");
        TratarErrosResponse(response);
        return await DeserializarObjetoResponse<ItemProdutoDto>(response);
    }
    
    public async Task<IEnumerable<ItemProdutoDto>> ObterItens(IEnumerable<Guid> ids)
    {
        var idsRequest = string.Join(",", ids);

        var response = await _httpClient.GetAsync($"/catalogo/produtos/lista/{idsRequest}/");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<List<ItemProdutoDto>>(response);
    }
}