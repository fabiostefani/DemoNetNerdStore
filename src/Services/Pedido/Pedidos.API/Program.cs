using Api.Core.Identidade;
using MediatR;
using Pedidos.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddMediatR(typeof(Program));
builder.Services.RegisterServices();
builder.Services.AddMessageBusConfiguration(builder.Configuration);
var app = builder.Build();

app.UseApiConfiguration(app.Environment);
app.UseSwaggerConfiguration();
app.MapControllers();
app.Run();
