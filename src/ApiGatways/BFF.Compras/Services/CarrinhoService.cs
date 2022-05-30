﻿using BFF.Compras.Extensions;
using BFF.Compras.Models;
using BFF.Compras.Services.Interfaces;
using Core.Communication;
using Microsoft.Extensions.Options;

namespace BFF.Compras.Services;

public class CarrinhoService : Service, ICarrinhoService
{
    private readonly HttpClient _httpClient;
    public CarrinhoService(HttpClient httpClient, 
                           IOptions<AppServicesSettings> settings)
    {
        if (string.IsNullOrEmpty(settings.Value.CarrinhoUrl) == false) 
            httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        _httpClient = httpClient;
    }

    public async Task<CarrinhoDto> ObterCarrinho()
    {
        var response = await _httpClient.GetAsync("/carrinho/");
        TratarErrosResponse(response);
        return await DeserializarObjetoResponse<CarrinhoDto>(response) ?? new CarrinhoDto();
    }

    public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDto produto)
    {
        var itemContent = ObterConteudo(produto);
        var response = await _httpClient.PostAsync("/carrinho/", itemContent);
        if(!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
        return RetornoOk();
    }

    public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto carrinho)
    {
        var itemContent = ObterConteudo(carrinho);
        var response = await _httpClient.PutAsync($"/carrinho/{produtoId}", itemContent);
        if(!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
        return RetornoOk();
    }

    public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
    {
        var response = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");
        if(!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
        return RetornoOk();
    }

    public async Task<ResponseResult> AplicarVoucherCarrinho(VocuherDto voucher)
    {
        var itemContent = ObterConteudo(voucher);
        var response = await _httpClient.PostAsync("/carrinho/aplicar-voucher", itemContent);
        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
        return RetornoOk();
    }
}