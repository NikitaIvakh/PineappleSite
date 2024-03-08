using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using PineappleSite.Gateway.Extensions;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.AddAppAuthentication();
applicationBuilder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
applicationBuilder.Services.AddOcelot(applicationBuilder.Configuration);

WebApplication webApplication = applicationBuilder.Build();

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