using Microsoft.AspNetCore.Authentication.Cookies;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Services;
using PineappleSite.Presentation.Services.Coupons;
using PineappleSite.Presentation.Services.Identities;
using System.Reflection;
using PineappleSite.Presentation.Services.Products;
using PineappleSite.Presentation.Services.ShoppingCarts;
using PineappleSite.Presentation.Services.Favorites;
using PineappleSite.Presentation.Services.Orders;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Localization.Routing;

WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.
applicationBuilder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();
applicationBuilder.Services.AddHttpContextAccessor();
applicationBuilder.Services.AddHttpClient();

applicationBuilder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

applicationBuilder.Services.AddHttpClient<ICouponClient, CouponClient>(couponClient => couponClient.BaseAddress = new Uri("https://localhost:7777"));
applicationBuilder.Services.AddHttpClient<IIdentityClient, IdentityClient>(identityClient => identityClient.BaseAddress = new Uri("https://localhost:7777"));
applicationBuilder.Services.AddHttpClient<IProductClient, ProductClient>(productClient => productClient.BaseAddress = new Uri("https://localhost:7777"));
applicationBuilder.Services.AddHttpClient<IFavoritesClient, FavoritesClient>(favoritesClient => favoritesClient.BaseAddress = new Uri("https://localhost:7777"));
applicationBuilder.Services.AddHttpClient<IShoppingCartClient, ShoppingCartClient>(cart => cart.BaseAddress = new Uri("https://localhost:7777"));
applicationBuilder.Services.AddHttpClient<IOrderClient, OrderClient>(order => order.BaseAddress = new Uri("https://localhost:7777"));

applicationBuilder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
applicationBuilder.Services.AddScoped<ICouponService, CouponService>();
applicationBuilder.Services.AddScoped<IUserService, UserService>();
applicationBuilder.Services.AddScoped<IProductService, ProductService>();
applicationBuilder.Services.AddScoped<IFavoriteService, FavoriteService>();
applicationBuilder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
applicationBuilder.Services.AddScoped<IOrderService, OrderService>();
applicationBuilder.Services.AddTransient<IIdentityService, IdentityService>();

applicationBuilder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Authenticate/Login";
        options.AccessDeniedPath = "/Authenticate/AccessDenied";
    });

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

WebApplication webApplication = applicationBuilder.Build();

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
webApplication.UseRouting();

webApplication.UseAuthentication();
webApplication.UseAuthorization();

webApplication.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

webApplication.Run();