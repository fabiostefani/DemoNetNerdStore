using Api.Core.Extensions;
using BFF.Compras.Services.gRPC;
using Carrinho.API.Services.gRPC;

namespace BFF.Compras.Configuration;

public static class GrpcConfig
{
    public static void ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<GrpcServiceInterceptor>();
        services.AddScoped<ICarrinhoGrpcService, CarrinhoGrpcService>();
        services.AddGrpcClient<CarrinhoCompras.CarrinhoComprasClient>(opt =>
        {
            opt.Address = new Uri(configuration["CarrinhoUrl"]);
        })
        .AddInterceptor<GrpcServiceInterceptor>()
        .AllowSelfSignedCertificate();
    }
}