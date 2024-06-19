using Identity.Application.Extensions;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Services;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Identity.Application.Features.Identities.Commands.Commands;

public sealed class LoginUserRequestHandler(
    UserManager<ApplicationUser> userManager,
    AuthRequestValidator authValidator,
    ITokenService tokenService,
    IConfiguration configuration) : IRequestHandler<LoginUserRequest, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await authValidator.ValidateAsync(request.AuthRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                var existErrorMessages = new Dictionary<string, List<string>>
                {
                    { "EmailAddress", validationResult.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "Password", validationResult.Errors.Select(key => key.ErrorMessage).ToList() },
                };

                foreach (var error in existErrorMessages)
                {
                    if (existErrorMessages.TryGetValue(error.Key, out var message))
                    {
                        return new Result<AuthResponseDto>
                        {
                            ValidationErrors = message,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("AccountLoginError", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<AuthResponseDto>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("AccountLoginError", ErrorMessage.Culture),
                };
            }

            var user = await userManager.FindByEmailAsync(request.AuthRequest.EmailAddress.ToLower());

            if (user is null)
            {
                return new Result<AuthResponseDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            var isValidPassword =
                await userManager.CheckPasswordAsync(user, request.AuthRequest.Password.Trim());

            if (!isValidPassword)
            {
                return new Result<AuthResponseDto>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("AccountLoginError", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("AccountLoginError", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var roles = await userManager.GetRolesAsync(user);

            var accessToken = tokenService.CreateToken(user, roles);
            user.RefreshToken = configuration.GenerateRefreshToken();
            user.RefreshTokenExpiresTime = DateTime.UtcNow.AddDays(
                int.Parse(configuration.GetSection("Jwt:RefreshTokenValidityInDays").Value!));

            var outputUser = new AuthResponseDto
            (
                FirstName: user.FirstName.Trim(),
                LastName: user.LastName.Trim(),
                UserName: user.UserName!.Trim(),
                EmailAddress: user.Email!.Trim().ToLower(),
                JwtToken: accessToken,
                RefreshToken: user.RefreshToken
            );

            await userManager.UpdateAsync(user);

            return new Result<AuthResponseDto>
            {
                Data = outputUser,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage = SuccessMessage.ResourceManager.GetString("SuccessfullyLogin", ErrorMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<AuthResponseDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}