using Favourites.Application.Services;
using Favourites.Domain.Entities.Favourite;
using Favourites.Domain.Interfaces.Repositories;
using Favourites.Domain.Interfaces.Services;
using Favourites.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Favourites.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureServices();
            services.ConfigureDatabaseConnectionString(configuration);
            services.ConfigureMigration();
        }

        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<FavouritesHeader>, BaseRepository<FavouritesHeader>>();
            services.AddScoped<IBaseRepository<FavouritesDetails>, BaseRepository<FavouritesDetails>>();
            services.AddScoped<IProductService, ProductService>();
        }

        private static void ConfigureDatabaseConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("FavouriteDbConnectionString"));
            });
        }

        private static void ConfigureMigration(this IServiceCollection services)
        {
            var scope = services.BuildServiceProvider();
            using var serviceProvider = scope.CreateScope();
            var db = serviceProvider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }
    }
}