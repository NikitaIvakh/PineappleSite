using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Identities.Commands.Commands;

public sealed class RevokeTokensRequestHandler(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context)
    : IRequestHandler<RevokeTokensRequest, CollectionResult<Unit>>
{
    public async Task<CollectionResult<Unit>> Handle(RevokeTokensRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var users = await userManager.Users.ToListAsync(cancellationToken);

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
                await userManager.UpdateAsync(user);
            }

            await context.SaveChangesAsync(cancellationToken);

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