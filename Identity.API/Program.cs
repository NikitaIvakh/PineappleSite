using Identity.Infrastructure;
using Identity.Application;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
applicationBuilder.Services.AddEndpointsApiExplorer();
applicationBuilder.Services.AddSwaggerGen();

applicationBuilder.Services.ConfigureIdentityService(applicationBuilder.Configuration);
applicationBuilder.Services.ConfigureApplicationService();

applicationBuilder.Services.AddCors(key =>
{
    key.AddPolicy("CorsPolicy",
        applicationBuilder => applicationBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

WebApplication webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseAuthentication();
webApplication.UseHttpsRedirection();
webApplication.UseRouting();
webApplication.UseAuthorization();
webApplication.UseCors();
webApplication.UseStaticFiles();
webApplication.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

webApplication.Run();