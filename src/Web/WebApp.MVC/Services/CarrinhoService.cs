﻿using Microsoft.Extensions.Options;
using WebApp.MVC.Extensions;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services;

public class CarrinhoService : Service, ICarrinhoService
{
    private readonly HttpClient _httpClient;

    public CarrinhoService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        if (settings.Value.CarrinhoUrl != null) 
            httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        _httpClient = httpClient;
    }

    public async Task<CarrinhoViewModel?> ObterCarrinho()
    {
        var response = await _httpClient.GetAsync("/carrinho/");
        TratarErrosResponse(response);
        return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
    }
    
    public async Task<ResponseResult?> AdicionarItemCarrinho(ItemProdutoViewModel produto)
    {
        var itemContent = ObterConteudo(produto);

        var response = await _httpClient.PostAsync("/carrinho/", itemContent);

        if(!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<ResponseResult?> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto)
    {
        var itemContent = ObterConteudo(produto);

        var response = await _httpClient.PutAsync($"/carrinho/{produto.ProdutoId}", itemContent);

        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<ResponseResult?> RemoverItemCarrinho(Guid produtoId)
    {
        var response = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");

        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
}