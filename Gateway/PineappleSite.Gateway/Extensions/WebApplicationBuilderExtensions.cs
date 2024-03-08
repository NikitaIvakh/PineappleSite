using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PineappleSite.Gateway.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {
            var settingsSession = builder.Configuration.GetSection("JwtSettings");

            var secret = settingsSession.GetValue<string>("Key");
            var issuer = settingsSession.GetValue<string>("Issuer");
            var audience = settingsSession.GetValue<string>("Audience");

            var secretKey = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(key =>
            {
                key.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                key.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(key =>
            {
                key.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateAudience = true,
                    ValidAudience = audience,
                };
            });

            return builder;
        }
    }
}