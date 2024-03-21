using Coupon.API;
using Coupon.Application.DependencyInjection;
using Coupon.Infrastructure.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Stripe;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureApplicationService();
applicationBuilder.Services.ConfigureInfrastructureServive(applicationBuilder.Configuration);

applicationBuilder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

StripeConfiguration.ApiKey = applicationBuilder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

applicationBuilder.Services.AddCors(key =>
{
    key.AddPolicy("CorsPolicy",
        applicationBuilder => applicationBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

applicationBuilder.Services.AddSwagger();
applicationBuilder.Services.AddMemoryCache();
applicationBuilder.Services.AddSwaggerAuthentication();
applicationBuilder.Services.AddAddAuthenticated(applicationBuilder.Configuration);

WebApplication webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();
webApplication.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

webApplication.UseSerilogRequestLogging();
webApplication.UseAuthorization();

webApplication.UseCors();
webApplication.MapControllers();

webApplication.Run();