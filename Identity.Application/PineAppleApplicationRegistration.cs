using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application
{
    public static class PineAppleApplicationRegistration
    {
        public static IServiceCollection ConfigureApplicationService(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });

            return services;
        }
    }
}