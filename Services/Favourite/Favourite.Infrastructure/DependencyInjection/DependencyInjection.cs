using Favourite.Domain.Entities;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Interfaces.Services;
using Favourite.Infrastructure.Health;
using Favourite.Infrastructure.Repository.Implement;
using Favourite.Infrastructure.Repository.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Favourite.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterServices(services, configuration);
        RegisterDatabase(services, configuration);
        RegisterMigrations(services);
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FavouriteDbConnectionString");

        services.AddScoped<IBaseRepository<FavouriteHeader>, BaseRepository<FavouriteHeader>>();
        services.AddScoped<IBaseRepository<FavouriteDetails>, BaseRepository<FavouriteDetails>>();
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped(scope => new DbConnectionFactory(connectionString!));
        services.AddHealthChecks().AddNpgSql(connectionString!).AddDbContextCheck<ApplicationDbContext>();
    }

    private static void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(config =>
        {
            config.UseNpgsql(configuration.GetConnectionString("FavouriteDbConnectionString"));
        });
    }

    private static void RegisterMigrations(IServiceCollection services)
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