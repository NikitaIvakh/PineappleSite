using Identity.Application.Features.Users.Requests.Queries;
using Identity.Application.Resources;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Queries;

public sealed class GetUsersProfileRequestHandler(UserManager<ApplicationUser> userManager, IMemoryCache memoryCache)
    : IRequestHandler<GetUsersProfileRequest, CollectionResult<GetUsersProfileDto>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<CollectionResult<GetUsersProfileDto>> Handle(GetUsersProfileRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out IReadOnlyCollection<GetUsersProfileDto>? usersProfile))
            {
                return new CollectionResult<GetUsersProfileDto>()
                {
                    Data = usersProfile,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("UsersSuccessfullyGeted", SuccessMessage.Culture)
                };
            }

            var users = await userManager.Users.ToListAsync(cancellationToken);

            if (users.Count == 0)
            {
                return new CollectionResult<GetUsersProfileDto>()
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            var getUsersProfile = new List<GetUsersProfileDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                getUsersProfile.Add(new GetUsersProfileDto
                (
                    UserId: user.Id,
                    FirstName: user.FirstName,
                    LastName: user.LastName,
                    UserName: user.UserName,
                    EmailAddress: user.Email,
                    Description: user.Description,
                    Age: user.Age,
                    ImageUrl: user.ImageUrl,
                    ImageLocalPath: user.ImageLocalPath,
                    CreatedTime: user.CreatedTime,
                    ModifiedTime: user.ModifiedTime,
                    Role: roles
                ));
            }

            var getOrderedList = getUsersProfile.OrderByDescending(key => key.CreatedTime).ToList();

            return new CollectionResult<GetUsersProfileDto>()
            {
                Data = getOrderedList,
                Count = getOrderedList.Count,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UsersSuccessfullyGeted", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<GetUsersProfileDto>()
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.NoAction,
            };
        }
    }
}