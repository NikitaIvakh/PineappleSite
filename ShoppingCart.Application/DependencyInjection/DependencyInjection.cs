using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ShoppingCart.Application
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationService(this IServiceCollection services)
        {
            services.RegisterInits();
        }

        private static void RegisterInits(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        }
    }
}