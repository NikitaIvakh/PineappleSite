using Coupon.Application.DependencyInjection;
using Coupon.Infrastructure.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureApplicationService();
applicationBuilder.Services.ConfigureInfrastructureServive(applicationBuilder.Configuration);

//applicationBuilder.Host.UseSerilog((context, configuration) =>
//{
//    configuration.ReadFrom.Configuration(context.Configuration)
//        .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning))
//        .WriteTo.File(new JsonFormatter(), "./Logs/debug-logs-.json", rollingInterval: RollingInterval.Day)
//        .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error))
//        .WriteTo.File(new JsonFormatter(), "./Logs/error-logs-.json", rollingInterval: RollingInterval.Day)
//        .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug);
//});

applicationBuilder.Host.UseSerilog((context, logConfig) => logConfig.ReadFrom.Configuration(context.Configuration));

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