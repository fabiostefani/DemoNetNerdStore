using Carrinho.API.Model;
using Core.Utils;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Carrinho.API.Data;

public sealed class CarrinhoContext : DbContext
{
    public CarrinhoContext(DbContextOptions<CarrinhoContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<CarrinhoCliente> CarrinhoClientes { get; set; }
    public DbSet<CarrinhoItem> CarrinhoItens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.AplicarConfiguracaoVarchar();
        modelBuilder.AplicarClientSetNullForeignKey();
        ConfiguracaoCarrinhoCliente(modelBuilder);
        ConfiguracaoCarrinhoItem(modelBuilder);
    }

    private static void ConfiguracaoCarrinhoItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarrinhoCliente>()
            .HasMany(c => c.Itens)
            .WithOne(i => i.CarrinhoCliente)
            .HasForeignKey(c => c.CarrinhoId);
        modelBuilder.Entity<CarrinhoItem>()
            .HasKey(x => x.Id);
    }

    private static void ConfiguracaoCarrinhoCliente(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarrinhoCliente>()
            .HasIndex(c => c.ClienteId)
            .HasDatabaseName("IDX_CLiente");
        modelBuilder.Entity<CarrinhoCliente>()
            .Ignore(c => c.Voucher)
            .OwnsOne(c => c.Voucher, v =>
            {
                v.Property(vc => vc.Codigo)
                    .HasColumnName("VoucherCodigo")
                    .HasColumnType("varchar(50)");
                v.Property(vc => vc.TipoDesconto)
                    .HasColumnName("TipoDesconto");
                v.Property(vc => vc.Percentual)
                    .HasColumnName("Percentual");
                v.Property(vc => vc.ValorDesconto)
                    .HasColumnName("ValorDesconto");
            });
        modelBuilder.Entity<CarrinhoCliente>()
            .HasKey(x => x.Id);
    }
}