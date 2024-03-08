using Microsoft.Extensions.DependencyInjection;
using PineappleSite.Infrastructure.RabbitMQ.Common;
using PineappleSite.Infrastructure.RabbitMQ.Events;
using System.Reflection;

namespace ShoppingCart.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.RegisterServices();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly()]));

            services.AddScoped<IRabbitMQMessageSender, RabbitMQMessageSender>();
        }
    }
}