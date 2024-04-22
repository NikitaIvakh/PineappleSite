using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Identity.Application.Extensions;

namespace Identity.Application.Services;

public sealed class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateToken(ApplicationUser user, IEnumerable<IdentityRole<string>> roles)
    {
        var token = user.CreateClaims(roles).CreateJwtToken(configuration);
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}