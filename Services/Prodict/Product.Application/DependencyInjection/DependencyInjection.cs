using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain.DTOs;
using System.Reflection;
using Product.Application.Validations;

namespace Product.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void ConfigureApplicationService(this IServiceCollection services)
    {
        RegisterInits(services);
        RegisterValidations(services);
    }

    private static void RegisterInits(IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
    }

    private static void RegisterValidations(IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateProductDto>, CreateValidator>();
        services.AddScoped<IValidator<UpdateProductDto>, UpdateValidator>();
        services.AddScoped<IValidator<DeleteProductDto>, DeleteValidator>();
        services.AddScoped<IValidator<DeleteProductsDto>, DeleteProductsValidator>();
    }
}