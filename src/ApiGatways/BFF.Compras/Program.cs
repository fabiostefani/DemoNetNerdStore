using Api.Core.Identidade;
using BFF.Compras.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.RegisterServicecs();
builder.Services.AddMessageBusConfig(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerConfiguration();
app.UseApiConfiguration(app.Environment);
app.MapControllers();
app.Run();
