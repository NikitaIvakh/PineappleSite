using Microsoft.Extensions.DependencyInjection;
using PineappleSite.Infrastructure.RabbitMQ.Common;
using PineappleSite.Infrastructure.RabbitMQ.Events;
using System.Reflection;
using FluentValidation;
using ShoppingCart.Application.Mapping;

namespace ShoppingCart.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
        RegisterServices(services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);
        services.AddMediatR(config => config.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly()]));

        services.AddScoped<IRabbitMqMessageSender, RabbitMqMessageSender>();
    }
}