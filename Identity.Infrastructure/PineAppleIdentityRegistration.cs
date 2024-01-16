using Identity.Core.Entities.User;
using Microsoft.AspNetCore.Identity;
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
                config.UseNpgsql(configuration.GetConnectionString("IdentityConnextionString"),
                config => config.MigrationsAssembly(typeof(PineAppleIdentityDbContext).Assembly.FullName));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<PineAppleIdentityDbContext>().AddDefaultTokenProviders();
            services.AddScoped<RoleManager<IdentityRole>>();


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