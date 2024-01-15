using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
    public static class PineAppleIdentityRegistration
    {
        public static IServiceCollection ConfigureIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PineAppleIdentityDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("IdentityConnextionString"));
            });

            var scope = services.BuildServiceProvider();
            var score = services.BuildServiceProvider();
            using (var serviceProvider = score.CreateScope())
            {
                var context = serviceProvider.ServiceProvider.GetRequiredService<PineAppleIdentityDbContext>();
                context.Database.Migrate();
            }

            return services;
        }
    }
}