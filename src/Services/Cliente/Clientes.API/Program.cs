using Api.Core.Identidade;
using Clientes.API.Configuration;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);            
builder.Services.AddSwaggerConfiguration();
builder.Services.AddMediatR(typeof(Program));
builder.Services.RegisterServices();

var app = builder.Build();

app.UseApiConfiguration(app.Environment);
app.UseSwaggerConfiguration();

// app.UseAuthorization();

app.MapControllers();

app.Run();