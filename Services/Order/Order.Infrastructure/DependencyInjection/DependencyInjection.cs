using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Domain.Entities;
using Order.Domain.Interfaces.Repository;
using Order.Domain.Interfaces.Services;
using Order.Infrastructure.Health;
using Order.Infrastructure.Repository.Implementation;
using Order.Infrastructure.Repository.Services;

namespace Order.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        RegisterServices(services, configuration);
        RegisterDbConnectionString(services, configuration);
        ConfigureMigration(services);
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrderDbConnectionString");

        services.AddScoped<IBaseRepository<OrderHeader>, BaseRepository<OrderHeader>>();
        services.AddScoped<IBaseRepository<OrderDetails>, BaseRepository<OrderDetails>>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped(scope => new DbConnectionFactory(connectionString!));
        services.AddHealthChecks().AddNpgSql(connectionString!).AddDbContextCheck<ApplicationDbContext>();
    }

    private static void RegisterDbConnectionString(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("OrderDbConnectionString"));
        });
    }

    private static void ConfigureMigration(IServiceCollection services)
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