using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Infrastructure.Health;
using ShoppingCart.Infrastructure.Repository.Implement;
using ShoppingCart.Infrastructure.Repository.Services;

namespace ShoppingCart.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices(configuration);
            services.RegisterDataBase(configuration);
            services.ApplyMigration();
        }

        private static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ShoppingCartDbConnection");

            services.AddScoped<IBaseRepository<CartHeader>, BaseRepository<CartHeader>>();
            services.AddScoped<IBaseRepository<CartDetails>, BaseRepository<CartDetails>>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICouponService, CouponService>();

            services.AddScoped(scope => new DbConnectionFactory(connectionString!));
            services.AddHealthChecks().AddNpgSql(connectionString!).AddDbContextCheck<ApplicationDbContext>();
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

            if (db.Database.GetPendingMigrations().Any())
            {
                db.Database.Migrate();
            }
        }
    }
}