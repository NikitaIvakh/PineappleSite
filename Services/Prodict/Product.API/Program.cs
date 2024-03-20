using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Product.API;
using Product.Application.DependencyInjection;
using Product.Infrastructure.DependencyInjection;
using Serilog;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.AddCors(key =>
{
    key.AddPolicy("CorsPolicy",
        applicationBuilder => applicationBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

applicationBuilder.Services.ConfigureInfrastructureService(applicationBuilder.Configuration);
applicationBuilder.Services.ConfigureApplicationService();
applicationBuilder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

applicationBuilder.Services.AddSwagger();
applicationBuilder.Services.AddMemoryCache();
applicationBuilder.Services.AddAppAuthenticate(applicationBuilder.Configuration);

WebApplication webApplication = applicationBuilder.Build();

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

webApplication.UseRouting();
webApplication.UseAuthorization();

webApplication.UseCors();
webApplication.UseStaticFiles();
webApplication.MapControllers();

webApplication.Run();