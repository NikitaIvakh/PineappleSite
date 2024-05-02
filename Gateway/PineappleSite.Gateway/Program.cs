using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using PineappleSite.Gateway.Extensions;

var applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.AddAppAuthentication(applicationBuilder.Configuration);
applicationBuilder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
applicationBuilder.Services.AddOcelot(applicationBuilder.Configuration);

var webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();
webApplication.UseRouting();
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.MapControllers();
webApplication.UseOcelot().GetAwaiter().GetResult();
webApplication.Run();