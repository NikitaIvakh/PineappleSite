using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain.Entities.Producrs;
using Product.Domain.Interfaces;
using Product.Infrastructure.Health;
using Product.Infrastructure.Repository;

namespace Product.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegsiterServices(configuration);
            services.RegisterDatabaseConnectionString(configuration);
            services.ApplyMigration();
        }

        private static void RegsiterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PineAppleProductDbContextConnectionString");

            services.AddScoped<IBaseRepository<ProductEntity>, BaseRepository<ProductEntity>>();
            services.AddScoped(scop => new DbConnectionFactory(connectionString));
            services.AddHealthChecks().AddNpgSql(connectionString).AddDbContextCheck<ApplicationDbContext>();
        }

        private static void RegisterDatabaseConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("PineAppleProductDbContextConnectionString"));
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