using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Infrastructure.Repository.Implement;

namespace ShoppingCart.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices();
            services.RegisterDataBase(configuration);
            services.ApplyMigration();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<CartHeader>, BaseRepository<CartHeader>>();
            services.AddScoped<IBaseRepository<CartDetails>, BaseRepository<CartDetails>>();
        }

        private static void RegisterDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("ShoppingCartDbConnection"));
            });
        }

        private static void ApplyMigration(this IServiceCollection services)
        {
            var scope = services.BuildServiceProvider();
            using var serviceProvider = scope.CreateScope();
            var db = serviceProvider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }
    }
}