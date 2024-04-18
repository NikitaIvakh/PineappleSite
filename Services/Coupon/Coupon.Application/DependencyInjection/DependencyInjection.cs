using Coupon.Application.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Coupon.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
        services.RegistersInit();
    }

    private static void RegistersInit(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}