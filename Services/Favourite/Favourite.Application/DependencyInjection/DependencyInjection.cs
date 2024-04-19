using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Favourite.Application.Mapping;

namespace Favourite.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
        RegisterInits(services);
    }

    private static void RegisterInits(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}