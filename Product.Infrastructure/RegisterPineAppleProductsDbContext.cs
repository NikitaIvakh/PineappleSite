using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Interfaces;

namespace Product.Infrastructure
{
    public static class RegisterPineAppleProductsDbContext
    {
        public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PineAppleProductsDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("PineAppleProductDbContextConnectionString"));
            });

            services.AddScoped<IProductDbContext, PineAppleProductsDbContext>();

            var scope = services.BuildServiceProvider();
            using (var serviceProvider = scope.CreateScope())
            {
                var db = serviceProvider.ServiceProvider.GetRequiredService<PineAppleProductsDbContext>();
                db.Database.Migrate();
            }

            return services;
        }
    }
}