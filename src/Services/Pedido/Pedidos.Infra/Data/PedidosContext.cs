using Core.Data;
using Core.DomainObjects;
using Core.Mediator;
using Core.Message;
using Core.Utils;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Vouchers;

namespace Pedidos.Infra.Data;

public class PedidosContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;
    public PedidosContext(DbContextOptions<PedidosContext> contextOptions, IMediatorHandler mediatorHandler)
        :base(contextOptions)
    {
        _mediatorHandler = mediatorHandler;
    }

    public DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AplicarConfiguracaoVarchar();
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        var sucesso = await base.SaveChangesAsync() > 0;
        if (sucesso) await _mediatorHandler.PublicarEventos(this);
        return sucesso;
    }
}