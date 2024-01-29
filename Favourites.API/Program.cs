using Favourites.API;
using Favourites.Application.DependencyInjection;
using Favourites.Infrastructure.DependencyInjection;
using Serilog;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureApplicationService();
applicationBuilder.Services.ConfigureInfrastructureService(applicationBuilder.Configuration);
applicationBuilder.Host.UseSerilog((context, logConfig) => logConfig.ReadFrom.Configuration(context.Configuration));

applicationBuilder.Services.AddHttpClient("Product", key => key.BaseAddress = new Uri(applicationBuilder.Configuration["ServiceUrls:Product"]));

applicationBuilder.Services.AddSwagger();

WebApplication webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();
webApplication.UseSerilogRequestLogging();
webApplication.UseAuthorization();
webApplication.MapControllers();
webApplication.Run();