using Microsoft.OpenApi.Models;

namespace ShoppingCart.API
{
    public static class Startup
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSwaggerGen();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Микросераис для работы с корзиной",
                    Description = "Микросераис для работы с корзиной",
                    TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nikita Ivakh",
                        Email = "nikita.ivakh7@gmail.com",
                    },
                });

                config.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Микросераис для работы с корзиной",
                    Description = "Микросераис для работы с корзиной",
                    TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nikita Ivakh",
                        Email = "nikita.ivakh7@gmail.com",
                    },
                });
            });
        }
    }
}