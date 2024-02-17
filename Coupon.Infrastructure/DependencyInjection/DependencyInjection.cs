using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Infrastructure.Health;
using Coupon.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Coupon.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServive(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CouponConnectionString");
            services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });

            services.RepositoriesInit(configuration);
            services.MigrationsInit();
        }

        private static void RepositoriesInit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBaseRepository<CouponEntity>, BaseRepository<CouponEntity>>();

            services.AddHealthChecks().AddCheck<DatabaseHealthCheck>("custom-postgresql", HealthStatus.Unhealthy);
            services.AddScoped(sp => new DbConnectionFactory(configuration.GetConnectionString("CouponConnectionString")));
        }

        private static void MigrationsInit(this IServiceCollection services)
        {
            var score = services.BuildServiceProvider();
            using var serviceProvider = score.CreateScope();
            var context = serviceProvider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }
    }
}