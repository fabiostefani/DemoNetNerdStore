using Microsoft.Extensions.Options;
using WebApp.MVC.Extensions;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services;

public class CatalogoService : Service, ICatalogoService
{
    private readonly HttpClient _httpClient;
    
    public CatalogoService(HttpClient httpClient,
        IOptions<AppSettings> settings)
    {
        if (settings.Value.CatalogoUrl != null) 
            httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        _httpClient = httpClient;
    }
    public async Task<IEnumerable<ProdutoViewModel>?> ObterTodos()
    {
        var response = await _httpClient.GetAsync("/catalogo/produtos/");
        TratarErrosResponse(response);
        return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
    }

    public async Task<ProdutoViewModel?> ObterPorId(Guid id)
    {
        var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");
        TratarErrosResponse(response);
        return await DeserializarObjetoResponse<ProdutoViewModel>(response);
    }
}