using Identity.Domain.Entities.Users;
using Identity.Infrastructure.Health;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterServices(services);
            RegisterDBConnectionString(services, configuration);
            ConfigureAuthentication(services, configuration);
            ApplyMigration(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<string>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddScoped<RoleManager<IdentityRole<string>>>();
        }

        private static void RegisterDBConnectionString(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("IdentityConnextionString");

            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseNpgsql(connectionString);
            });

            services.AddScoped(scope => new DbConnectionFactory(connectionString!));
            services.AddHealthChecks().AddNpgSql(connectionString!).AddDbContextCheck<ApplicationDbContext>();
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
                AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                    };
                });

            services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
        }

        private static void ApplyMigration(IServiceCollection services)
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