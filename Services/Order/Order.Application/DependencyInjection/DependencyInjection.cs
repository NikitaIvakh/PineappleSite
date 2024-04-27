using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Order.Application.Mapping;

namespace Order.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
        services.RegisterInits();
    }

    private static void RegisterInits(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMediatR(config => config.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly()]));
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
    }
}