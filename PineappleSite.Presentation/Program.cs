using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Services;
using PineappleSite.Presentation.Services.Coupons;
using PineappleSite.Presentation.Services.Identities;
using System.Reflection;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.
applicationBuilder.Services.AddControllersWithViews();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddHttpClient();

applicationBuilder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

applicationBuilder.Services.AddHttpClient<ICouponClient, CouponClient>(couponClient => couponClient.BaseAddress = new Uri("https://localhost:7149"));
applicationBuilder.Services.AddHttpClient<IIdentityClient, IdentityClient>(identityClient => identityClient.BaseAddress = new Uri("https://localhost:7133"));
applicationBuilder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

applicationBuilder.Services.AddScoped<ICouponService, CouponService>();
applicationBuilder.Services.AddScoped<IUserService, UserService>();

applicationBuilder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

applicationBuilder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
applicationBuilder.Services.AddTransient<IIdentityService, IdentityService>();

WebApplication webApplication = applicationBuilder.Build();

// Configure the HTTP request pipeline.
if (!webApplication.Environment.IsDevelopment())
{
    webApplication.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    webApplication.UseHsts();
}

webApplication.UseCookiePolicy();
webApplication.UseHttpsRedirection();
webApplication.UseStaticFiles();

webApplication.UseRouting();

webApplication.UseAuthentication();
webApplication.UseAuthorization();

webApplication.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

webApplication.Run();