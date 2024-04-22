using Identity.Application.Extecsions;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Identity.Application.Features.Identities.Commands.Commands;

public sealed class RefreshTokenRequestHandler(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ApplicationDbContext context) : IRequestHandler<RefreshTokenRequest, Result<ObjectResult>>
{
    public async Task<Result<ObjectResult>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var accessToken = request.TokenModelDto.AccessToken;
            var refreshToken = request.TokenModelDto.RefreshToken;
            var principal = configuration.ClaimsPrincipalFromExpiredToken(accessToken);

            if (principal is null)
            {
                return new Result<ObjectResult>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("InvalidAccessOrRefreshToken", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("InvalidAccessOrRefreshToken", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var userName = principal.Identity!.Name;
            var user = await userManager.FindByNameAsync(userName!);

            if (user is null || user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiresTime <= DateTime.UtcNow)
            {
                return new Result<ObjectResult>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("InvalidAccessOrRefreshToken", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("InvalidAccessOrRefreshToken", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var newAccessToken = configuration.CreateJwtToken(principal.Claims.ToList());
            var newRefreshToken = configuration.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await context.SaveChangesAsync(cancellationToken);

            return new Result<ObjectResult>
            {
                Data = new ObjectResult(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    refreshToken = newRefreshToken,
                }),

                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("TokensUpdatedSuccessfully", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<ObjectResult>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}