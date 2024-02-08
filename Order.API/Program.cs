using Order.Application.DependencyInjection;
using Order.Infrastructure.DependencyInjection;
using Stripe;
using System.Text.Json.Serialization;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureApplicationServices();
applicationBuilder.Services.ConfigureInfrastructureServices(applicationBuilder.Configuration);

applicationBuilder.Services.AddHttpClient("Product", key => key.BaseAddress = new Uri(applicationBuilder.Configuration["ServiceUrls:Product"]));
applicationBuilder.Services.AddHttpClient("Coupon", key => key.BaseAddress = new Uri(applicationBuilder.Configuration["ServiceUrls:Coupon"]));

StripeConfiguration.ApiKey = applicationBuilder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

WebApplication webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();

webApplication.UseAuthorization();

webApplication.MapControllers();

webApplication.Run();