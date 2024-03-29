﻿using System.Text.Json.Serialization;
using Carrinho.API.Model.Validacoes;

namespace Carrinho.API.Model;

public class CarrinhoItem
{
    public Guid Id { get; private set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; }= string.Empty;
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
    public string Imagem { get; set; } = string.Empty;
    public Guid? CarrinhoId { get; set; }
    [JsonIgnore]
    public CarrinhoCliente? CarrinhoCliente { get; set; }
    
    public CarrinhoItem()
    {
        Id = Guid.NewGuid();
    }

    internal void AssociarItem(Guid carrinhoId)
        => CarrinhoId = carrinhoId;

    internal decimal CalcularValor()
        => Quantidade * Valor;

    internal void AdicionarUnidades(int unidades)
        => Quantidade += unidades;

    internal void AtualizarUnidades(int unidades)
        => Quantidade = unidades;
    
    internal bool EhValido()
        => new ItemCarrinhoValidation().Validate(this).IsValid;

    internal void AssociarCarrinho(Guid id)
        => CarrinhoId = id;
}