using Microsoft.AspNetCore.Localization;
using System.Globalization;
using PineappleSite.Presentation;
using PineappleSite.Presentation.Middleware;

var applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.
applicationBuilder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddHttpClient();
applicationBuilder.Services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());

applicationBuilder.Services.ConfigurePresentationServices(applicationBuilder.Configuration);

applicationBuilder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

applicationBuilder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ru-RU"),
        new CultureInfo("en-US"),
    };

    options.DefaultRequestCulture = new RequestCulture("ru-RU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var webApplication = applicationBuilder.Build();

var supportedCultures = new[]
{
    new CultureInfo("ru-RU"),
    new CultureInfo("en-US"),
};

webApplication.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

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

webApplication.UseAuthentication();
webApplication.UseAuthorization();

webApplication.UseRouting();

webApplication.UseMiddleware<TokenExpirationMiddleware>();

webApplication.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

webApplication.Run();