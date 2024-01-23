using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Services;
using ShoppingCart.Application.Services.IServices;
using System.Reflection;

namespace ShoppingCart.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICouponService, CouponService>();

            return services;
        }
    }
}