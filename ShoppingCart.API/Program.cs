using Serilog;
using ShoppingCart.Application;
using ShoppingCart.Infrastructure;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureApplicationService();
applicationBuilder.Services.ConfigureShoppingCartService(applicationBuilder.Configuration);

applicationBuilder.Services.AddHttpClient("Product", key => key.BaseAddress = new Uri(applicationBuilder.Configuration["ServiceUrls:Product"]));
applicationBuilder.Services.AddHttpClient("Coupon", key => key.BaseAddress = new Uri(applicationBuilder.Configuration["ServiceUrls:Coupon"]));
applicationBuilder.Host.UseSerilog((context, logConfig) => logConfig.ReadFrom.Configuration(context.Configuration));

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