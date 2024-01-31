using Identity.Application.Services;
using Identity.Domain.Entities.Users;
using Identity.Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices();
            services.RegisterDBConnectionString(configuration);
            services.ApplyMigration();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<PineAppleIdentityDbContext>().AddDefaultTokenProviders();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<ITokenProvider, TokenProvider>();
        }

        private static void RegisterDBConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PineAppleIdentityDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("IdentityConnextionString"),
                config => config.MigrationsAssembly(typeof(PineAppleIdentityDbContext).Assembly.FullName));
            });
        }

        private static void ApplyMigration(this IServiceCollection services)
        {
            var scope = services.BuildServiceProvider();
            using var serviceProvider = scope.CreateScope();
            var context = serviceProvider.ServiceProvider.GetRequiredService<PineAppleIdentityDbContext>();
            context.Database.Migrate();
        }
    }
}