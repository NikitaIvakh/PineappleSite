using Order.Application.DependencyInjection;
using Order.Infrastructure.DependencyInjection;
using Order.API;
using Carter;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var applicationBuilder = WebApplication.CreateBuilder(args);

applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddSwaggerGen();
applicationBuilder.Services.AddCarter();

applicationBuilder.Services.ConfigureApplicationServices();
applicationBuilder.Services.ConfigureInfrastructureServices(applicationBuilder.Configuration);

applicationBuilder.Services.AddMemoryCache();
applicationBuilder.Services.AddDependency(applicationBuilder, applicationBuilder.Configuration);

var webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

webApplication.MapCarter();
webApplication.UseHttpsRedirection();
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.Run();