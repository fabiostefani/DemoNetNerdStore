using Core.Data;
using Core.DomainObjects;
using Core.Mediator;
using Core.Message;
using Core.Utils;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Pedidos;
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
    public DbSet<Pedido?> Pedidos { get; set; }
    public DbSet<PedidoItem?> PedidoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AplicarConfiguracaoVarchar();
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosContext).Assembly);
        modelBuilder.AplicarClientSetNullForeignKey();
        modelBuilder.HasSequence<int>("minhasequencia").StartsAt(1000).IncrementsBy(1);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        AtualizarDataCadastro();
        var sucesso = await base.SaveChangesAsync() > 0;
        if (sucesso) await _mediatorHandler.PublicarEventos(this);
        return sucesso;
    }

    private void AtualizarDataCadastro()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataCadastro").IsModified = false;
            }
        }
    }
}