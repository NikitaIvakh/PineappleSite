using FluentValidation;
using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;
using Identity.Core.Interfaces;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Identity.Infrastructure
{
    public static class PineAppleIdentityRegistration
    {
        public static IServiceCollection ConfigureIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddDbContext<PineAppleIdentityDbContext>(config =>
            {
                config.UseNpgsql(configuration.GetConnectionString("IdentityConnextionString"),
                config => config.MigrationsAssembly(typeof(PineAppleIdentityDbContext).Assembly.FullName));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<PineAppleIdentityDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });


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