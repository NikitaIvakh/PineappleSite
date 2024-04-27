using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Stripe;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;
using static Order.API.Utility.StaticDetails;
using File = System.IO.File;

namespace Order.API;

public static class Startup
{
    public static void AddDependency(this IServiceCollection services, WebApplicationBuilder applicationBuilder,
        IConfiguration configuration)
    {
        ConfigureServices(services, configuration);
        ConfigureStripeKey(applicationBuilder);
        ConfigureSerilog(applicationBuilder);
        AddSwagger(services);
        AddAppAuthenticated(services, configuration);
        AddSwaggerAuthentication(services);
        AddAuthenticatePolicy(services);
    }

    private static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("Product",
            key => key.BaseAddress = new Uri(configuration["ServiceUrls:Product"]!));

        services.AddHttpClient("Coupon",
            key => key.BaseAddress = new Uri(configuration["ServiceUrls:Coupon"]!));

        services.AddHttpClient("User",
            key => key.BaseAddress = new Uri(configuration["ServiceUrls:User"]!));
    }

    private static void ConfigureStripeKey(this WebApplicationBuilder applicationBuilder)
    {
        StripeConfiguration.ApiKey = applicationBuilder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
    }

    private static void ConfigureSerilog(this WebApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Host.UseSerilog((context, logConfig) =>
        {
            logConfig.ReadFrom.Configuration(context.Configuration);
            logConfig.WriteTo.Console();
        });
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Микросервис для работы с заказами",
                Description = "Микросервис для работы с заказами",
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
                Title = "Микросервис для работы с заказами",
                Description = "Микросервис для работы с заказами",
                TermsOfService = new Uri("https://github.com/NikitaIvakh"),
                Contact = new OpenApiContact
                {
                    Name = "Nikita Ivakh",
                    Email = "nikita.ivakh7@gmail.com",
                },
            });
        });
    }

    private static void AddAppAuthenticated(this IServiceCollection services, IConfiguration configuration)
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

    private static void AddSwaggerAuthentication(this IServiceCollection services)
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

    private static void AddAuthenticatePolicy(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AdministratorPolicy, policy => policy.RequireRole(RoleAdministrator))
            .AddPolicy(UserAndAdministratorPolicy, policy => policy.RequireRole(RoleUser, RoleAdministrator));
    }
}