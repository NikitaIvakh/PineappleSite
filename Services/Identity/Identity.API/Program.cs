using Carter;
using HealthChecks.UI.Client;
using Identity.API;
using Identity.Application.DependencyInjection;
using Identity.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddSwaggerGen();
applicationBuilder.Services.AddCarter();
applicationBuilder.Services.AddAntiforgery();

applicationBuilder.Services.ConfigureInfrastructureService(applicationBuilder.Configuration);
applicationBuilder.Services.ConfigureApplicationService(applicationBuilder.Configuration);
applicationBuilder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

applicationBuilder.Services.AddSwagger();
applicationBuilder.Services.AddMemoryCache();

var webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.MapCarter();
webApplication.UseHttpsRedirection();
webApplication.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

webApplication.UseRouting();
webApplication.UseAntiforgery();
webApplication.UseAuthentication();
webApplication.UseStaticFiles();
webApplication.Run();