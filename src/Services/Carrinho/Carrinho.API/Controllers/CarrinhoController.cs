﻿using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Api.Core.Controllers;
using Api.Core.Usuario;
using Carrinho.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Carrinho.API.Model;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Carrinho.API.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    private readonly IAspNetUser _aspNetUser;
    private readonly CarrinhoContext _context;

    public CarrinhoController(IAspNetUser aspNetUser,
                              CarrinhoContext context)
    {
        _aspNetUser = aspNetUser;
        _context = context;
    }
    
    [HttpGet("carrinho")]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return await ObterCarrinhoCliente() ?? new CarrinhoCliente();
    }
    [HttpPost("carrinho")]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();
        if (carrinho == null)
            ManipularNovoCarrinho(item);
        else
            ManipularCarrinhoExistente(carrinho, item);
        
        if (!OperacaoValida()) return CustomResponse();
        await PersistirDados();
        return CustomResponse();
    }

    [HttpPut("carrinho/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();
        var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho, item);
        if (itemCarrinho == null) 
            return CustomResponse();
        if (carrinho == null) 
            return CustomResponse();
        carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);
        ValidarCarrinho(carrinho);
        if (!OperacaoValida()) return CustomResponse();
        _context.CarrinhoItens.Update(itemCarrinho);
        _context.CarrinhoClientes.Update(carrinho);
        await PersistirDados();
        return CustomResponse();
    }

    [HttpDelete("carrinho/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var carrinho = await ObterCarrinhoCliente();
        var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);
        if (itemCarrinho == null)
            return CustomResponse();
        if (carrinho == null) 
            return CustomResponse();
        ValidarCarrinho(carrinho);
        if (!OperacaoValida()) return CustomResponse();
        carrinho.RemoverItem(itemCarrinho);
        _context.CarrinhoItens.Remove(itemCarrinho);
        _context.CarrinhoClientes.Update(carrinho);
        await PersistirDados();
        return CustomResponse();
    }

    [HttpPost]
    [Route("carrinho/aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher(Voucher voucher)
    {
        var carrinho = await ObterCarrinhoCliente();
        if (carrinho is null) 
        {
            AdicionarErroProcessamento("Não foi possível obter o carrinho do cliente.");
            return CustomResponse();
        }
        carrinho?.AplicarVoucher(voucher);
        _context.CarrinhoClientes.Update(carrinho);
        var result = await _context.SaveChangesAsync();
        if (result <= 0) AdicionarErroProcessamento("Não foi possível persistir os dados no banco");
        return CustomResponse();
    }

    private async Task<CarrinhoCliente?> ObterCarrinhoCliente()
        => await _context.CarrinhoClientes
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.ClienteId == _aspNetUser.ObterUserId());
    
    private void ManipularNovoCarrinho(CarrinhoItem item)
    {
        var carrinho = new CarrinhoCliente(_aspNetUser.ObterUserId());
        carrinho.AdicionarItem(item);
        ValidarCarrinho(carrinho);
        _context.CarrinhoClientes.Add(carrinho);
    }
    
    private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
    {
        bool produtoItemExistente = carrinho.CarrinhoItemExistente(item);
        carrinho.AdicionarItem(item);
        ValidarCarrinho(carrinho);
        if (produtoItemExistente)
        {
            _context.CarrinhoItens.Update(carrinho.ObterPorProdutoId(item.ProdutoId));
        }
        else
        {
            _context.CarrinhoItens.Add(item);
        }
        _context.CarrinhoClientes.Update(carrinho);
    }

    private async Task<CarrinhoItem> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente? carrinho, CarrinhoItem? item = null)
    {
        if (item != null && produtoId != item.ProdutoId)
        {
            AdicionarErroProcessamento("O item não corresponde ao informado.");
            return null;
        }
        if (carrinho == null)
        {
            AdicionarErroProcessamento("Carrinho não encontrado.");
            return null;
        }
        var itemCarrinho = await _context.CarrinhoItens
            .FirstOrDefaultAsync(i => i.CarrinhoId == carrinho.Id && i.ProdutoId == produtoId);
        if (itemCarrinho == null || !carrinho.CarrinhoItemExistente(itemCarrinho))
        {
            AdicionarErroProcessamento("O item não está no carrinho.");
            return null;
        }
        return itemCarrinho;
    }

    private async Task PersistirDados()
    {
        var result = await _context.SaveChangesAsync();
        if (result<=0)
            AdicionarErroProcessamento("Não foi possível persistir os dados no banco.");
    }

    private bool ValidarCarrinho(CarrinhoCliente carrinho)
    {
        if (carrinho.EhValido())
            return true;
        carrinho.ValidationResult.Errors.ToList().ForEach(e => AdicionarErroProcessamento(e.ErrorMessage));
        return false;
    }
}