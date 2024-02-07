using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Favourite.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.RegisterInits();
            services.RegisterServices();
        }

        private static void RegisterInits(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly()]));
        }

        private static void RegisterServices(this IServiceCollection services)
        {

        }
    }
}