using Identidade.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddMessageBusConfiguration(builder.Configuration);


var app = builder.Build();

app.UseSwaggerConfiguration(app.Environment);
app.UseApiConfiguration(app.Environment);
app.MapControllers();

app.Run();
