using Identity.Domain.Entities.Users;
using Identity.Infrastructure.Health;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterServices(services);
        RegisterDbConnectionString(services, configuration);
        ApplyMigration(services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<string>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false; 
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5; 
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>();

        services.AddScoped<RoleManager<IdentityRole<string>>>();
    }

    private static void RegisterDbConnectionString(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentityConnectionString");

        services.AddDbContext<ApplicationDbContext>(config => { config.UseNpgsql(connectionString); });

        services.AddScoped(scope => new DbConnectionFactory(connectionString!));
        services.AddHealthChecks().AddNpgSql(connectionString!).AddDbContextCheck<ApplicationDbContext>();
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