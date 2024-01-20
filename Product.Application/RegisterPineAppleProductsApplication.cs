using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Product.Application
{
    public static class RegisterPineAppleProductsApplication
    {
        public static IServiceCollection ConfigureApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}