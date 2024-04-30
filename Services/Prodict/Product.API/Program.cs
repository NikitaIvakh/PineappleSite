using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Product.API;
using Product.Application.DependencyInjection;
using Product.Infrastructure.DependencyInjection;
using Serilog;

var applicationBuilder = WebApplication.CreateBuilder(args);

applicationBuilder.Services.AddControllers();
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddSwaggerGen();
applicationBuilder.Services.AddCarter();

applicationBuilder.Services.ConfigureInfrastructureService(applicationBuilder.Configuration);
applicationBuilder.Services.ConfigureApplicationService();
applicationBuilder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

applicationBuilder.Services.AddMemoryCache();
applicationBuilder.Services.AddDependencyServices(applicationBuilder.Configuration);

var webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseAuthentication();
webApplication.UseHttpsRedirection();
webApplication.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

webApplication.MapCarter();
webApplication.UseRouting();
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.UseStaticFiles();
webApplication.MapControllers();
webApplication.Run();