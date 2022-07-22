using Api.Core.Identidade;
using Carrinho.API.Configuration;
using Carrinho.API.Services.gRPC;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);            
builder.Services.AddSwaggerConfiguration();
builder.Services.RegisterServices();
builder.Services.AddMessageBusConfiguration(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiConfiguration(app.Environment);
app.UseSwaggerConfiguration();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<CarrinhoGrpcService>().RequireCors("Total");
});
//app.MapControllers();
app.Run();
