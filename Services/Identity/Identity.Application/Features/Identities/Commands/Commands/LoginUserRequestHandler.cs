using Identity.Application.Extensions;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Services;
using Identity.Application.Validators;
using Identity.Domain.Enum;
using Identity.Domain.Interfaces;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Application.Features.Identities.Commands.Commands;

public sealed class LoginUserRequestHandler(
    IUserRepository userRepository,
    AuthRequestValidator authValidator,
    ITokenService tokenService,
    IConfiguration configuration) : IRequestHandler<LoginUserRequest, Result<string>>
{
    public async Task<Result<string>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
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
                        return new Result<string>
                        {
                            ValidationErrors = message,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("AccountLoginError", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("AccountLoginError", ErrorMessage.Culture),
                };
            }

            var user = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.Email == request.AuthRequest.EmailAddress, cancellationToken);

            if (user is null)
            {
                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            var isValidPassword =
                await userRepository.CheckPasswordAsync(user, request.AuthRequest.Password.Trim());

            if (!isValidPassword)
            {
                return new Result<string>
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

            var roles = await userRepository.GetUserRolesAsync(user);

            var accessToken = tokenService.CreateToken(user, roles);
            user.RefreshToken = configuration.GenerateRefreshToken();
            user.RefreshTokenExpiresTime = DateTime.UtcNow.AddDays(
                int.Parse(configuration.GetSection("Jwt:RefreshTokenValidityInDays").Value!));

            return new Result<string>
            {
                Data = accessToken,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage = SuccessMessage.ResourceManager.GetString("SuccessfullyLogin", ErrorMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<string>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}