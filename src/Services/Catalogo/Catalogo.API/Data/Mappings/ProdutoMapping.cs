using Catalogo.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogo.API.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome)
            .IsRequired()
            .HasColumnType("varchar(250)")
            .HasMaxLength(250);

            builder.Property(x => x.Descricao)
            .IsRequired()
            .HasColumnType("varchar(500)")
            .HasMaxLength(500);

            builder.Property(x => x.Imagem)
            .IsRequired()
            .HasColumnType("varchar(250)")
            .HasMaxLength(250);
            builder.ToTable("produtos");
        }
    }
}