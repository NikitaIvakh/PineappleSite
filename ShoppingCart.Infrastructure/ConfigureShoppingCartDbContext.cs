using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Interfaces;

namespace ShoppingCart.Infrastructure
{
    public static class ConfigureShoppingCartDbContext
    {
        public static IServiceCollection ConfigureShoppingCartService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShoppingCartDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("ShoppingCartDbConnectionString"));
            });

            services.AddScoped<ICartHeaderDbContext, ShoppingCartDbContext>();
            services.AddScoped<ICartDetailsDbContext, ShoppingCartDbContext>();

            var scope = services.BuildServiceProvider();
            using (var serviceProvider = scope.CreateScope())
            {
                var db = serviceProvider.ServiceProvider.GetRequiredService<ShoppingCartDbContext>();
                db.Database.Migrate();
            }

            return services;
        }
    }
}