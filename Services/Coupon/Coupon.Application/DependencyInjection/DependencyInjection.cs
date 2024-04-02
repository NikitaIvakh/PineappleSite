using Coupon.Application.Mapping;
using Coupon.Application.Validations;
using Coupon.Domain.DTOs;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Coupon.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.RegistersInit();
            services.ServicesInit();
        }

        private static void RegistersInit(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        }

        private static void ServicesInit(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateCouponDto>, CreateValidator>();
            services.AddScoped<IValidator<UpdateCouponDto>, UpdateValidator>();
            services.AddScoped<IValidator<DeleteCouponDto>, DeleteValidator>();
            services.AddScoped<IValidator<DeleteCouponsDto>, DeleteCouponsValidator>();
        }
    }
}