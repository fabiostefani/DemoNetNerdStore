using Api.Core.Identidade;
using Microsoft.EntityFrameworkCore;
using Pagamentos.API.Data;
using Pagamentos.API.Facade;

namespace Pagamentos.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddDbContext<PagamentosContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();
        services.Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
    }

    public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseAuthConfiguration();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}