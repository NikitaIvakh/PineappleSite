﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using static Product.API.Utility.StaticDetails;

namespace Product.API;

public static class Startup
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Микросервис для работы с продуктами",
                Description = "Микросервис для работы с продуктами",
                TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                Contact = new OpenApiContact
                {
                    Name = "Nikita Ivakh",
                    Email = "nikita.ivakh7@gmail.com",
                },
            });

            option.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "Микросервис для работы с продуктами",
                Description = "Микросервис для работы с продуктами",
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
            .SetDefaultPolicy(
                new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
    }

    public static void AddSwaggerAuthenticate(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
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
                            Id = JwtBearerDefaults.AuthenticationScheme,
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