using Favourite.API;
using Favourite.Application.DependencyInjection;
using Favourite.Infrastructure.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);

builder.Services.AddHttpClient("Product", key => key.BaseAddress = new Uri(builder.Configuration["ServiceUrls:Product"]));
builder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

builder.Services.AddSwagger();
builder.Services.AddMemoryCache();
builder.Services.AddAppAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthorization();

app.MapControllers();

app.Run();