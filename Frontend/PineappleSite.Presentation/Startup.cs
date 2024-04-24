using Microsoft.AspNetCore.Authentication.Cookies;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Services;
using PineappleSite.Presentation.Services.Coupons;
using PineappleSite.Presentation.Services.Favourite;
using PineappleSite.Presentation.Services.Identities;
using PineappleSite.Presentation.Services.Orders;
using PineappleSite.Presentation.Services.Products;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation;

public static class Startup
{
    public static void ConfigurePresentationServices(this IServiceCollection services)
    {
        ConfigureHttpClients(services);
        ConfigureServices(services);
        ConfigureCookieFile(services);
    }

    private static void ConfigureHttpClients(IServiceCollection services)
    {
        services.AddHttpClient<ICouponClient, CouponClient>(couponClient =>
            couponClient.BaseAddress = new Uri("https://localhost:7777"));

        services.AddHttpClient<IIdentityClient, IdentityClient>(identityClient =>
            identityClient.BaseAddress = new Uri("https://localhost:7777"));

        services.AddHttpClient<IProductClient, ProductClient>(productClient =>
            productClient.BaseAddress = new Uri("https://localhost:7777"));

        services.AddHttpClient<IFavouriteClient, FavouriteClient>(favoritesClient =>
            favoritesClient.BaseAddress = new Uri("https://localhost:7777"));

        services.AddHttpClient<IShoppingCartClient, ShoppingCartClient>(cart =>
            cart.BaseAddress = new Uri("https://localhost:7777"));

        services.AddHttpClient<IOrderClient, OrderClient>(order =>
            order.BaseAddress = new Uri("https://localhost:7777"));
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ILocalStorageService, LocalStorageService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IFavouriteService, FavouriteService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddTransient<IIdentityService, IdentityService>();
    }

    private static void ConfigureCookieFile(IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Authenticate/Login";
                options.AccessDeniedPath = "/Authenticate/AccessDenied";
                options.LogoutPath = "/Authenticate/Logout";
                options.Cookie.Name = "AuthenticateCookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            });
    }
}