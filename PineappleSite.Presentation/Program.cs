using PineappleSite.Presentation.Services.Coupons;
using System.Reflection;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.
applicationBuilder.Services.AddControllersWithViews();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddHttpClient();

applicationBuilder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

applicationBuilder.Services.AddHttpClient<ICouponClient, CouponClient>(couponClient => couponClient.BaseAddress = new Uri("https://localhost:7149"));

WebApplication webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (!webApplication.Environment.IsDevelopment())
{
    webApplication.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    webApplication.UseHsts();
}

webApplication.UseHttpsRedirection();
webApplication.UseStaticFiles();

webApplication.UseRouting();

webApplication.UseAuthorization();

webApplication.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

webApplication.Run();