using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Favourites.Application
{
    public static class RegisterApplicationService
    {
        public static IServiceCollection ConfigureApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}