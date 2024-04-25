using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain.Interfaces;
using Product.Infrastructure.Health;
using Product.Infrastructure.Repository;

namespace Product.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        RegsiterServices(services, configuration);
        RegisterDatabaseConnectionString(services, configuration);
        ApplyMigration(services);
    }

    private static void RegsiterServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PineAppleProductDbContextConnectionString");

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped(scop => new DbConnectionFactory(connectionString!));
        services.AddHealthChecks().AddNpgSql(connectionString!).AddDbContextCheck<ApplicationDbContext>();
    }

    private static void RegisterDatabaseConnectionString(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(config =>
        {
            config.UseNpgsql(configuration.GetConnectionString("PineAppleProductDbContextConnectionString"));
        });
    }

    private static void ApplyMigration(IServiceCollection services)
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