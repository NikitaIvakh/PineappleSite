using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Domain.Enum;
using Identity.Domain.Interfaces;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Identities.Commands.Commands;

public sealed class RevokeTokensRequestHandler(IUserRepository userRepository)
    : IRequestHandler<RevokeTokensRequest, CollectionResult<Unit>>
{
    public async Task<CollectionResult<Unit>> Handle(RevokeTokensRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var users = await userRepository.GetUsers().ToListAsync(cancellationToken);

            if (users.Count == 0)
            {
                return new CollectionResult<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            foreach (var user in users)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiresTime = DateTime.UtcNow;
                await userRepository.UpdateUserAsync(user, cancellationToken);
            }

            return new CollectionResult<Unit>()
            {
                Data = [Unit.Value],
                Count = users.Count,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("TokensSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<Unit>()
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}