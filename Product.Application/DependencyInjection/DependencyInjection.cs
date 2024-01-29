using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.DTOs.Validator;
using Product.Domain.DTOs;
using System.Reflection;

namespace Product.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationService(this IServiceCollection services)
        {
            services.RegisterInits();
            services.RegisterValidations();
        }

        private static void RegisterInits(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        }

        private static void RegisterValidations(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateProductDto>, ICreateProductDtoValidator>();
            services.AddScoped<IValidator<UpdateProductDto>, IUpdateProductDtoValidator>();
            services.AddScoped<IValidator<DeleteProductDto>, IDeleteProductDtoValidator>();
            services.AddScoped<IValidator<DeleteProductsDto>, IDeleteProductsDtoValidator>();
        }
    }
}