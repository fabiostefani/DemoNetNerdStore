using Api.Core.Controllers;
using BFF.Compras.Models;
using BFF.Compras.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace BFF.Compras.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly ICatalogoService _catalogoService;
    private readonly IPedidoService _pedidoService;

    public CarrinhoController(ICarrinhoService carrinhoService,
                              ICatalogoService catalogoService,
                              IPedidoService pedidoService)
    {
        _carrinhoService = carrinhoService;
        _catalogoService = catalogoService;
        _pedidoService = pedidoService;
    }
    
    [HttpGet]
    [Route("compras/carrinho")]
    public async Task<IActionResult> Index()
    {
        return CustomResponse(await _carrinhoService.ObterCarrinho());
    }
    
    [HttpGet]
    [Route("compras/carrinho-quantidade")]
    public async Task<int> ObterQuantidadeCarrinho()
    {
        var quantidade = await _carrinhoService.ObterCarrinho();
        return quantidade?.Itens.Sum(x => x.Quantidade) ?? 0;
    }
    
    [HttpPost]
    [Route("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDto itemProduto)
    {
        var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);
        if (!OperacaoValida()) return CustomResponse();
        AtualizarInformacoesProduto();
        var resposta = await _carrinhoService.AdicionarItemCarrinho(itemProduto);
        return CustomResponse(resposta);
        
        void AtualizarInformacoesProduto()
        {
            itemProduto.Nome = produto.Nome;
            itemProduto.Valor = produto.Valor;
            itemProduto.Imagem = produto.Imagem;
        }
    }

    [HttpPut]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto itemProduto)
    {
        var produto = await _catalogoService.ObterPorId(produtoId);
        await ValidarItemCarrinho(produto, itemProduto.Quantidade);
        if (!OperacaoValida()) return CustomResponse();
        var resposta = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);
        return CustomResponse(resposta);
    }
    
    [HttpDelete]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> DeletarItemCarrinho(Guid produtoId)
    {
        var respota = await _carrinhoService.RemoverItemCarrinho(produtoId);
        return CustomResponse(respota);
    }
    
    [HttpPost("compras/carrinho/aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
    {
        var voucher = await _pedidoService.ObterVocuherPorCodigo(voucherCodigo);
        if (voucher is null)
        {
            AdicionarErroProcessamento("Voucher inválido ou não encontrado.");
            return CustomResponse();
        }

        var resposta = await _carrinhoService.AplicarVoucherCarrinho(voucher);
        return CustomResponse(resposta);
    }

    private async Task ValidarItemCarrinho(ItemProdutoDto? produto, int quantidade)
    {
        if (produto == null) AdicionarErroProcessamento("Produto inexistente!");
        if (quantidade < 1) AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto?.Nome}");
        var carrinho = await _carrinhoService.ObterCarrinho();
        var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produto?.Id);
        if (itemCarrinho != null && itemCarrinho.Quantidade + quantidade > produto?.QuantidadeEstoque)
        {
            AdicionarErroProcessamento($"O produto {produto?.Nome} possui {produto?.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
            return;
        }
        if (quantidade > produto?.QuantidadeEstoque)
            AdicionarErroProcessamento($"O produto {produto?.Nome} possui {produto?.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
    }

}