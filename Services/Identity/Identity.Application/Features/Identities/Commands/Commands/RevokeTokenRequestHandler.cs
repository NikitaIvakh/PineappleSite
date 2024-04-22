using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Domain.Enum;
using Identity.Domain.Interfaces;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Identities.Commands.Commands;

public sealed class RevokeTokenRequestHandler(
    IUserRepository userRepository)
    : IRequestHandler<RevokeTokenRequest, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RevokeTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetAll(cancellationToken)
                .FirstOrDefaultAsync(key => key.UserName == request.UserName, cancellationToken: cancellationToken);

            if (user is null)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiresTime = DateTime.UtcNow;
            await userRepository.UpdateUserAsync(user, cancellationToken);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("TokenSuccessfullyDeleted", ErrorMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}