using Catalogo.API.Models;
using Core.Data;
using Core.Message;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.API.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public CatalogoContext(DbContextOptions<CatalogoContext> options)
            :base(options)
        {
            
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                {
                    if (string.IsNullOrEmpty(property.GetColumnType()))
                    {
                        property.SetColumnType("varchar(100)");
                    }                    
                }
                
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}