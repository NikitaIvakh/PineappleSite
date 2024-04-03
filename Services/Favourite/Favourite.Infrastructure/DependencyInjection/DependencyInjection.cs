using Favourite.Domain.Entities;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Interfaces.Services;
using Favourite.Infrastructure.Health;
using Favourite.Infrastructure.Repository.Implement;
using Favourite.Infrastructure.Repository.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Favourite.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices(configuration);
            services.RegisterDatabase(configuration);
            services.RegisterMigrations();
        }

        private static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("FavouriteDbConnectionString");

            services.AddScoped<IBaseRepositiry<FavouriteHeader>, BaseRepository<FavouriteHeader>>();
            services.AddScoped<IBaseRepositiry<FavouriteDetails>, BaseRepository<FavouriteDetails>>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped(scope => new DbConnectionFactory(connectionString));
            services.AddHealthChecks().AddNpgSql(connectionString).AddDbContextCheck<ApplicationDbContext>();
        }

        private static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("FavouriteDbConnectionString"));
            });
        }

        private static void RegisterMigrations(this IServiceCollection services)
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