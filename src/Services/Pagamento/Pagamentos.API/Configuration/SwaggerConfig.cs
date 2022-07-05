using Microsoft.OpenApi.Models;

namespace Pagamentos.API.Configuration;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc(name: "v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Loja do Fabio Enterprise Pagamento API",
                Description = "Pagamentos",
                Contact = new OpenApiContact() { Name = "Fabio de Stefani", Email = "fabiostefani@gmail.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });

        });
    }

    public static void UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });
    }
}