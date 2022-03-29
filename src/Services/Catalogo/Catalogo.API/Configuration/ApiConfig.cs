using Catalogo.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogoContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Total", configurePolicy: builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });

            services.AddEndpointsApiExplorer();
            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("Total");

            return app;
        }
    }
}