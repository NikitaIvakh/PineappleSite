using Coupon.Application.Interfaces;
using Coupon.Domain.Interfaces.Database;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coupon.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServive(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("CouponConnectionString"));
            });

            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICouponDbContext, ApplicationDbContext>();

            var score = services.BuildServiceProvider();
            using (var serviceProvider = score.CreateScope())
            {
                var context = serviceProvider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            return services;
        }
    }
}