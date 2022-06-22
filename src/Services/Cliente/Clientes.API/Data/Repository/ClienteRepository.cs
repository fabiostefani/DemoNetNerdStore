﻿using Clientes.API.Models;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Clientes.API.Data.Repository;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientesContext _context;
    public ClienteRepository(ClientesContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Adicionar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
    }

    public async Task<IEnumerable<Cliente?>> ObterTodos()
    {
        return await _context.Clientes.AsNoTracking().ToListAsync();
    }

    public async Task<Cliente?> ObterPorCpf(string cpf)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
    }

    public async Task<Endereco> ObterEnderecoPorId(Guid id)
    {
        return await _context.Enderecos.FirstOrDefaultAsync(e => e.ClienteId == id);
    }
    
    public void AdicionarEndereco(Endereco endereco)
    {
        _context.Enderecos.Add(endereco);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}