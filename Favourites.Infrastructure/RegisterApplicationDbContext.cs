using Favourites.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Favourites.Infrastructure
{
    public static class RegisterApplicationDbContext
    {
        public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("FavouriteDbConnectionString"));
            });

            services.AddScoped<IFavoutiteHeaderDbContext, ApplicationDbContext>();
            services.AddScoped<IFavoutiteDetailsDbContext, ApplicationDbContext>();

            var scope = services.BuildServiceProvider();
            using (var serviceProvider = scope.CreateScope())
            {
                var db = serviceProvider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }

            return services;
        }
    }
}