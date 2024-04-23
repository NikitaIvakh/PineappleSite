using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Identity.API;

public static class Startup
{
    public static void AddSwaggerConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        AddSwaggerUi(services);
        AddSwaggerAuthenticate(services, configuration);
    }

    private static void AddSwaggerUi(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Микросервис для авторизации",
                Description = "Микросервис для авторизации и для работы с пользователями",
                TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                Contact = new OpenApiContact
                {
                    Name = "Nikita Ivakh",
                    Email = "nikita.ivakh7@gmail.com",
                },
            });

            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "Микросервис для авторизации",
                Description = "Микросервис для авторизации и для работы с пользователями",
                TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                Contact = new OpenApiContact
                {
                    Name = "Nikita Ivakh",
                    Email = "nikita.ivakh7@gmail.com",
                },
            });
        });
    }

    private static void AddSwaggerAuthenticate(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Issuer"],
                    ValidAudience = configuration["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secret"]!)),
                };
            });

        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
    }
}