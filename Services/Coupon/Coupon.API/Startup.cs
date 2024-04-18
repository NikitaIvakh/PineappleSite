using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using static Coupon.API.Utility.StaticDetails;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

namespace Coupon.API;

public static class Startup
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Микросервис для работы с купонами",
                Description = "Микросервис для работы с купонами",
                TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                Contact = new OpenApiContact
                {
                    Name = "Nikita Ivakh",
                    Email = "nikita.ivakh7@gmail.com",
                },

                License = new OpenApiLicense
                {
                    Name = "Mit Licence",
                },
            });

            option.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "Микросервис для работы с купонами",
                Description = "Микросервис для работы с купонами",
                TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                Contact = new OpenApiContact
                {
                    Name = "Nikita Ivakh",
                    Email = "nikita.ivakh7@gmail.com",
                },

                License = new OpenApiLicense
                {
                    Name = "Mit Licence",
                },
            });
        });
    }

    public static void AddAddAuthenticated(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthenticationScheme;
                options.DefaultChallengeScheme = AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
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
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(AuthenticationScheme).RequireAuthenticatedUser().Build());
    }

    public static void AddSwaggerAuthentication(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(name: AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme,
                        },

                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },

                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            else
            {
                Console.WriteLine($"XML-файл документации не найден: {xmlPath}");
            }
        });
    }

    public static void AddAuthenticatePolicy(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(name: AdministratorPolicy, policy => { policy.RequireRole(RoleAdministrator); });

        services.AddAuthorizationBuilder()
            .AddPolicy(name: UserAndAdministratorPolicy,
                policy => { policy.RequireRole(RoleUser, RoleAdministrator); });
    }
}