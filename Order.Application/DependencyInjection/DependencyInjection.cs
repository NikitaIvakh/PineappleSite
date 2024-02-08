using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Order.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.RegisterInits();
        }

        private static void RegisterInits(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly()]));
        }
    }
}