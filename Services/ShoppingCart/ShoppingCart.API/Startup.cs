using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ShoppingCart.API
{
    public static class Startup
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Микросервис для работы с корзиной",
                    Description = "Микросервис для работы с корзиной",
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
                    Title = "Микросервис для работы с корзиной",
                    Description = "Микросервис для работы с корзиной",
                    TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nikita Ivakh",
                        Email = "nikita.ivakh7@gmail.com",
                    },
                });
            });
        }

        public static void AddAppAuthenticate(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                    };
                });

            services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
        }
    }
}