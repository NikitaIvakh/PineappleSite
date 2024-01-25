using Product.Application;
using Product.Infrastructure;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureInfrastructureService(applicationBuilder.Configuration);
applicationBuilder.Services.ConfigureApplicationService();

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