using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Utils;

public static class ModelBuilderExtensions
{
    public static ModelBuilder AplicarConfiguracaoVarchar(this ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                     e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
        {
            SetarColunaVarchar(property);
        }
        return modelBuilder;
    }

    private static void SetarColunaVarchar(IMutableProperty property)
    {
        if (!string.IsNullOrEmpty(property.GetColumnType())) 
            return;
        property.SetColumnType("varchar(100)");
    }

    public static ModelBuilder AplicarClientSetNullForeignKey(this ModelBuilder modelBuilder)
    {
        foreach (var relationShip in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e=>e.GetForeignKeys()))
        {
            relationShip.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        return modelBuilder;
    }
}