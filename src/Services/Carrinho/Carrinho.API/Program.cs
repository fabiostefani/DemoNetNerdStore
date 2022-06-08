using Api.Core.Identidade;
using Carrinho.API.Configuration;

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
app.MapControllers();
app.Run();
