using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ShoppingCart.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {

        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly()]));
        }
    }
}