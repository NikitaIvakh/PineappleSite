using Favourites.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Favourites.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationService(this IServiceCollection services)
        {
            services.RegisterInits();
            services.ServicesInits();
        }

        private static void RegisterInits(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly()]));
        }

        private static void ServicesInits(this IServiceCollection services)
        {

        }
    }
}