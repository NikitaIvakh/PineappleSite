using Carter;
using ShoppingCart.Infrastructure.DependencyInjection;
using ShoppingCart.Application.DependencyInjection;
using ShoppingCart.API;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);

builder.Services.AddHttpClient("Product",
    key => key.BaseAddress = new Uri(builder.Configuration["ServiceUrls:Product"]!));

builder.Services.AddHttpClient("Coupon",
    key => key.BaseAddress = new Uri(builder.Configuration["ServiceUrls:Coupon"]!));

builder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

builder.Services.AddSwagger();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerAuthenticate();
builder.Services.AddAppAuthenticate(builder.Configuration);
builder.Services.AddAuthenticatePolicy();

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
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.MapCarter();
app.UseAuthentication();
app.UseAuthorization();
app.Run();