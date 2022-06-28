using System.Net;
using BFF.Compras.Extensions;
using BFF.Compras.Models;
using BFF.Compras.Services.Interfaces;
using Core.Communication;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class PedidoService : Service, IPedidoService
{
    private readonly HttpClient _httpClient;
    public PedidoService(HttpClient httpClient, 
                         IOptions<AppServicesSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.PedidoUrl) == false) 
            httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        _httpClient = httpClient;
    }

    public async Task<VocuherDto?> ObterVoucherPorCodigo(string codigo)
    {
        var response = await _httpClient.GetAsync($"/voucher/{codigo}");
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        TratarErrosResponse(response);
        return await DeserializarObjetoResponse<VocuherDto>(response);
    }
    
    public async Task<ResponseResult> FinalizarPedido(PedidoDto pedido)
    {
        var pedidoContent = ObterConteudo(pedido);

        var response = await _httpClient.PostAsync("/pedido/", pedidoContent);

        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<PedidoDto> ObterUltimoPedido()
    {
        var response = await _httpClient.GetAsync("/pedido/ultimo/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<PedidoDto>(response);
    }

    public async Task<IEnumerable<PedidoDto>> ObterListaPorClienteId()
    {
        var response = await _httpClient.GetAsync("/pedido/lista-cliente/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<List<PedidoDto>>(response);
    }
}