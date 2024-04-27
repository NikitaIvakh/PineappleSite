using Order.Application.DependencyInjection;
using Order.Infrastructure.DependencyInjection;
using Stripe;
using Order.API;
using System.Text.Json.Serialization;
using Carter;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var applicationBuilder = WebApplication.CreateBuilder(args);

applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddSwaggerGen();
applicationBuilder.Services.AddCarter();

applicationBuilder.Services.ConfigureApplicationServices();
applicationBuilder.Services.ConfigureInfrastructureServices(applicationBuilder.Configuration);

applicationBuilder.Services.AddHttpClient("Product",
    key => key.BaseAddress = new Uri(applicationBuilder.Configuration["ServiceUrls:Product"]!));

applicationBuilder.Services.AddHttpClient("Coupon",
    key => key.BaseAddress = new Uri(applicationBuilder.Configuration["ServiceUrls:Coupon"]!));

applicationBuilder.Services.AddHttpClient("User",
    key => key.BaseAddress = new Uri(applicationBuilder.Configuration["ServiceUrls:User"]!));

StripeConfiguration.ApiKey = applicationBuilder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

applicationBuilder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

applicationBuilder.Services.AddSwagger();
applicationBuilder.Services.AddMemoryCache();
applicationBuilder.Services.AddSwaggerAuthenticaton();
applicationBuilder.Services.AddAppAuthenticate(applicationBuilder.Configuration);
applicationBuilder.Services.AddAuthenticatePolicy();

var webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();
webApplication.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

webApplication.MapCarter();
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.Run();