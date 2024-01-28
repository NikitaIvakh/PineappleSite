using Coupon.API;
using Coupon.Application.DependencyInjection;
using Coupon.Infrastructure.DependencyInjection;
using Serilog;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureApplicationService();
applicationBuilder.Services.ConfigureInfrastructureServive(applicationBuilder.Configuration);
applicationBuilder.Host.UseSerilog((context, logConfig) => logConfig.ReadFrom.Configuration(context.Configuration));

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