using System.Net;
using Microsoft.Extensions.Options;
using Core.Communication;
using WebApp.MVC.Extensions;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services;

public class ClienteService : Service, IClienteService
{
    private readonly HttpClient _httpClient;

    public ClienteService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        if (!string.IsNullOrEmpty(settings.Value.ClienteUrl)) 
            httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
        _httpClient = httpClient;
    }

    public async Task<EnderecoViewModel?> ObterEndereco()
    {
        var response = await _httpClient.GetAsync("/cliente/endereco/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<EnderecoViewModel>(response);
    }

    public async Task<ResponseResult?> AdicionarEndereco(EnderecoViewModel endereco)
    {
        var enderecoContent = ObterConteudo(endereco);

        var response = await _httpClient.PostAsync("/cliente/endereco/", enderecoContent);

        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
}