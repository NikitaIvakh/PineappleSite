﻿using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using static Identity.API.Utility.StaticDetails;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

namespace Identity.API;

public static class Startup
{
    public static void AddSwaggerConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        AddSwaggerUi(services);
        AddSwaggerAuthenticate(services, configuration);
        AddSwaggerAuthorize(services);
        AddAuthPolicy(services);
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

    private static void AddSwaggerAuthorize(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(name: AuthenticationScheme,
                securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authoriation stirng as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });

            options.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
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

    private static void AddAuthPolicy(IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(name: AdministratorPolicy, policy => { policy.RequireRole(RoleAdministrator); });

        services.AddAuthorizationBuilder()
            .AddPolicy(name: UserAndAdministratorPolicy,
                policy => { policy.RequireRole(RoleUser, RoleAdministrator); });
    }
}