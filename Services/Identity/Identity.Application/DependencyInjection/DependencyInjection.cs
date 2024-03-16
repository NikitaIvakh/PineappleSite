using FluentValidation;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices(configuration);
            services.RegisterInts();
        }

        private static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddScoped<ITokenService, TokenService>();
        }

        private static void RegisterInts(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);
        }
    }
}