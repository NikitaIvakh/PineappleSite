using Carter;
using Coupon.Application.DependencyInjection;
using Coupon.Infrastructure.DependencyInjection;
using Serilog;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

builder.Services.AddMemoryCache();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureService(builder.Configuration);

builder.Host.UseSerilog((context, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
    logConfig.WriteTo.Console();
});

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

// TO serilog, swagger

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.Run();