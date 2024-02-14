using Coupon.API;
using Coupon.Application.DependencyInjection;
using Coupon.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Localization;
using Serilog;
using Stripe;
using System.Globalization;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureApplicationService();
applicationBuilder.Services.ConfigureInfrastructureServive(applicationBuilder.Configuration);
applicationBuilder.Host.UseSerilog((context, logConfig) => logConfig.ReadFrom.Configuration(context.Configuration));

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

applicationBuilder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Coupon.Application.Resources.Features.Coupons.Handlers.Commands";
});

applicationBuilder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ru-RU"),
        new CultureInfo("en-US"),
    };

    options.DefaultRequestCulture = new RequestCulture("ru-RU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

WebApplication webApplication = applicationBuilder.Build();

var supportedCultures = new[]
{
    new CultureInfo("ru-RU"),
    new CultureInfo("en-US"),
};

webApplication.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();
webApplication.UseSerilogRequestLogging();
webApplication.UseAuthorization();
webApplication.UseCors();
webApplication.MapControllers();
webApplication.Run();