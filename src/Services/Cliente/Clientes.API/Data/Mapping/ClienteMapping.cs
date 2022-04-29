using Clientes.API.Models;
using Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clientes.API.Data.Mapping;

public class ClienteMapping : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey((x=>x.Id));
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");
        builder.OwnsOne(c => c.Cpf, tf =>
        {
            tf.Property(x => x.Numero)
                .IsRequired()
                .HasMaxLength(Cpf.CpfMaxLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.CpfMaxLength})");
        });
        builder.OwnsOne(c => c.Email, tf =>
        {
            tf.Property(c => c.Endereco)
                .IsRequired()
                .HasColumnName("Email")
                .HasMaxLength(Email.EnderecoMaxLength)
                .HasColumnType($"varchar({Email.EnderecoMaxLength})");

        });

        builder.HasOne(x => x.Endereco)
            .WithOne(x => x.Cliente);
        
        builder.ToTable("Clientes");
    }
}