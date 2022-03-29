using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Catalogo.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt => 
            {
                opt.SwaggerDoc(name: "v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Loja do Fabio Enterprise Catalogo API",
                    Description = "Catálogo de produtos da Loja",
                    Contact = new OpenApiContact() { Name = "Fabio de Stefani", Email = "fabiostefani@gmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });
                // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(opt => 
                {
                    opt.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "v1");
                    opt.RoutePrefix = string.Empty;
                });    
            }
            
            return app;
        }
    }
}