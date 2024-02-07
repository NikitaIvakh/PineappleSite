using Favourite.Domain.Entities;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Infrastructure.Repository.Implement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Favourite.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices();
            services.RegisterDatabase(configuration);
            services.RegisterMigrations();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepositiry<FavouriteHeader>, BaseRepository<FavouriteHeader>>();
            services.AddScoped<IBaseRepositiry<FavouriteDetails>, BaseRepository<FavouriteDetails>>();
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
            db.Database.Migrate();
        }
    }
}