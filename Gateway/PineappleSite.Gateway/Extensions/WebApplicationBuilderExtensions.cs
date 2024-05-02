using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

namespace PineappleSite.Gateway.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddAppAuthentication(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddAuthentication(key =>
        {
            key.DefaultAuthenticateScheme = AuthenticationScheme;
            key.DefaultChallengeScheme = AuthenticationScheme;
        }).AddJwtBearer(key =>
        {
            key.TokenValidationParameters = new TokenValidationParameters
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

        builder.Services.AddAuthorizationBuilder()
            .SetDefaultPolicy(
                new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
    }
}