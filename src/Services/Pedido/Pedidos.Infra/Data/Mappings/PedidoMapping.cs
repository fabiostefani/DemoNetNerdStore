using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pedidos.Domain.Pedidos;

namespace Pedidos.Infra.Data.Mappings;

public class PedidoMapping : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.HasKey(c => c.Id);
        builder.OwnsOne(p => p.Endereco, e =>
        {
            e.Property(pe => pe.Logradouro)
                .HasColumnName("Logradouro");
            e.Property(pe => pe.Numero)
                .HasColumnName("Numero");
            e.Property(pe => pe.Complemento)
                .HasColumnName("Complemento");
            e.Property(pe => pe.Bairro)
                .HasColumnName("Bairro");
            e.Property(pe => pe.Cep)
                .HasColumnName("Cep");
            e.Property(pe => pe.Cidade)
                .HasColumnName("Cidade");
            e.Property(pe => pe.Estado)
                .HasColumnName("Estado");
        });

        builder.Property(p => p.Codigo)
            .HasDefaultValueSql("nextval('minhasequencia')");

        builder.HasMany(p => p.PedidoItens)
            .WithOne(p => p.Pedido)
            .HasForeignKey(p => p.PedidoId);

        builder.ToTable("Pedidos");

    }
}