using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Domain.Entities.Cart;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Infrastructure.Repository;
using ShoppingCart.Infrastructure.Services;

namespace ShoppingCart.Infrastructure
{
    public static class DependencyInjection
    {
        public static void ConfigureShoppingCartService(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices();
            services.RegisterDBConnectionString(configuration);
            services.RegisterMigration();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<CartHeader>, BaseRepository<CartHeader>>();
            services.AddScoped<IBaseRepository<CartDetails>, BaseRepository<CartDetails>>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICouponService, CouponService>();
        }

        private static void RegisterDBConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("ShoppingCartDbConnectionString"));
            });
        }

        private static void RegisterMigration(this IServiceCollection services)
        {
            var scope = services.BuildServiceProvider();
            using var serviceProvider = scope.CreateScope();
            var db = serviceProvider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }
    }
}