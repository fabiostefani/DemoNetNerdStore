using Catalogo.API.Data;
using Catalogo.API.Data.Repositories;
using Catalogo.API.Models;

namespace Catalogo.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContext>();
            return services;
        }
    }
}