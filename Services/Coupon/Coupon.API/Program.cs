using Carter;
using Coupon.Application.DependencyInjection;
using Coupon.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

builder.Services.AddMemoryCache();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureService(builder.Configuration);

// TO serilog, swagger

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseHttpsRedirection();
app.Run();