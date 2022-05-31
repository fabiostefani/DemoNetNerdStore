using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api.Core.Identidade
{
    public static class JwtConfig
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection appSettingsSecction = configuration.GetSection(key: "AppSettings");
            services.Configure<AppSettings>(appSettingsSecction);
            AppSettings appSettings = appSettingsSecction.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Segredo);
            
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });
            return services;
        }

        public static IApplicationBuilder UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}