using Api.Core.Identidade;
using Api.Core.Usuario;
using Identidade.API.Services;
using NetDevPack.Security.Jwt.Core.Interfaces;

namespace Identidade.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<AuthenticationService>();
            services.AddControllers();
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

            app.UseAuthConfiguration();
            //localhost/jwts
            app.UseJwksDiscovery();
            
            return app;
        }
    }
}