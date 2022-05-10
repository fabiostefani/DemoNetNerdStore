using Clientes.API.Models;
using Core.Data;
using Core.Mediator;
using Core.Message;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Clientes.API.Data;

public sealed class ClientesContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;

    public ClientesContext(DbContextOptions<ClientesContext> options,
                          IMediatorHandler mediatorHandler)
        : base(options)
    {
        _mediatorHandler = mediatorHandler;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                     e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
        {
            if (string.IsNullOrEmpty(property.GetColumnType()))
            {
                property.SetColumnType("varchar(100)");
            }                    
        }

        foreach (var relationShip in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e=>e.GetForeignKeys()))
        {
            relationShip.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        var suscesso = await base.SaveChangesAsync() > 0;
        if (suscesso) 
            await _mediatorHandler.PublicarEventos(this);
        
        return suscesso;
    }
}