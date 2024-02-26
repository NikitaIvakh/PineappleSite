using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Infrastructure.Health;
using Coupon.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coupon.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureServive(this IServiceCollection services, IConfiguration configuration)
        {
            services.RepositoriesInit(configuration);
            services.RegisterConnectionString(configuration);
            services.MigrationsInit();
        }

        private static void RepositoriesInit(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CouponConnectionString");

            services.AddScoped<IBaseRepository<CouponEntity>, BaseRepository<CouponEntity>>();
            services.AddScoped(sp => new DbConnectionFactory(connectionString));
            services.AddHealthChecks().AddNpgSql(connectionString).AddDbContextCheck<ApplicationDbContext>();
        }

        private static void RegisterConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CouponConnectionString");
            services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });
        }

        private static void MigrationsInit(this IServiceCollection services)
        {
            var score = services.BuildServiceProvider();
            using var serviceProvider = score.CreateScope();
            var context = serviceProvider.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}