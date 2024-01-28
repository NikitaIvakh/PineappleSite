using Coupon.Application.DependencyInjection;
using Coupon.Infrastructure.DependencyInjection;
using Serilog;

namespace Coupon.API
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/CouponAPI.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Hello, world!");

            try
            {
                WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

                // Add services to the container.

                applicationBuilder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                applicationBuilder.Services.AddEndpointsApiExplorer();
                applicationBuilder.Services.AddSwaggerGen();

                applicationBuilder.Services.ConfigureApplicationService();
                applicationBuilder.Services.ConfigureInfrastructureServive(applicationBuilder.Configuration);
                applicationBuilder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

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
            }

            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
            }

            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}