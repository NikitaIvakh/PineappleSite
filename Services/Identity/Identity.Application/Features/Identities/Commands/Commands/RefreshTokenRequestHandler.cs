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

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class RefreshTokenRequestHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext context) : IRequestHandler<RefreshTokenRequest, Result<ObjectResult>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<ObjectResult>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var accessToken = request.TokenModelDto.AccessToken;
                var refreshToken = request.TokenModelDto.RefreshToken;
                var principal = _configuration.ClaimsPrincipalFromExpiredToken(accessToken);

                if (principal is null)
                {
                    return new Result<ObjectResult>
                    {
                        ErrorCode = (int)ErrorCodes.InvalidAccessOrRefreshToken,
                        ErrorMessage = ErrorMessage.InvalidAccessOrRefreshToken,
                        ValidationErrors = [ErrorMessage.InvalidAccessOrRefreshToken]
                    };
                }

                else
                {
                    var userName = principal.Identity!.Name;
                    var user = await _userManager.FindByNameAsync(userName!);

                    if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiresTime <= DateTime.UtcNow)
                    {
                        return new Result<ObjectResult>
                        {
                            ErrorCode = (int)ErrorCodes.InvalidAccessOrRefreshToken,
                            ErrorMessage = ErrorMessage.InvalidAccessOrRefreshToken,
                            ValidationErrors = [ErrorMessage.InvalidAccessOrRefreshToken]
                        };
                    }

                    else
                    {
                        var newAccessToken = _configuration.CreateJwtToken(principal.Claims.ToList());
                        var newRefreshToken = _configuration.GenerateRefreshToken();

                        user.RefreshToken = newRefreshToken;
                        await _context.SaveChangesAsync(cancellationToken);

                        return new Result<ObjectResult>
                        {
                            Data = new ObjectResult(new
                            {
                                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                                refreshToken = newRefreshToken,
                            }),

                            SuccessCode = (int)SuccessCode.Ok,
                            SuccessMessage = SuccessMessage.TokensUpdatedSuccessfully,
                        };
                    }
                }
            }

            catch
            {
                return new Result<ObjectResult>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}