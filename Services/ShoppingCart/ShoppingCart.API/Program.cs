using ShoppingCart.Infrastructure.DependencyInjection;
using ShoppingCart.Application.DependencyInjection;
using ShoppingCart.API;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);

builder.Services.AddHttpClient("Product", key => key.BaseAddress = new Uri(builder.Configuration["ServiceUrls:Product"]));
builder.Services.AddHttpClient("Coupon", key => key.BaseAddress = new Uri(builder.Configuration["ServiceUrls:Coupon"]));

builder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

builder.Services.AddSwagger();
builder.Services.AddMemoryCache();
builder.Services.AddAppAuthenticate(builder.Configuration);

builder.Services.AddCors(key =>
{
    key.AddPolicy("CorsPolicy",
        applicationBuilder => applicationBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

WebApplication app = builder.Build();

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

app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();