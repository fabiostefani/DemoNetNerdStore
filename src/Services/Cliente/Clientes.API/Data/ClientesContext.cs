using Clientes.API.Models;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Clientes.API.Data;

public sealed class ClientesContext : DbContext, IUnitOfWork
{
    public ClientesContext(DbContextOptions<ClientesContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Cliente?> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        return await base.SaveChangesAsync() > 0;
    }
}