using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
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
            var connectionString = configuration.GetConnectionString("CouponConnectionString");
            services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });

            services.RepositoriesInit();
            services.MigrationsInit();
        }

        private static void RepositoriesInit(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<CouponEntity>, BaseRepository<CouponEntity>>();
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