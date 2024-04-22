using Identity.Application.Features.Users.Requests.Queries;
using Identity.Application.Resources;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Queries;

public sealed class GetUserRequestHandler(
    UserManager<ApplicationUser> userManager,
    IMemoryCache memoryCache) : IRequestHandler<GetUserRequest, Result<GetUserDto>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<GetUserDto>> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out GetUserDto? userWithRoles))
            {
                return new Result<GetUserDto>
                {
                    Data = userWithRoles,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage = SuccessMessage.UserSuccessfullyGeted,
                };
            }

            var user = await userManager.FindByIdAsync(request.UserId);

            if (user is null)
            {
                return new Result<GetUserDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            var roles = await userManager.GetRolesAsync(user);

            var getUserWithRoles = new GetUserDto(user.Id, user.FirstName, user.LastName, user.UserName!, user.Email!,
                roles, user.CreatedTime, user.ModifiedTime);

            memoryCache.Remove(CacheKey);

            return new Result<GetUserDto>
            {
                Data = getUserWithRoles,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UserSuccessfullyGeted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new Result<GetUserDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}