using MediatR;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using Microsoft.EntityFrameworkCore;
using Identity.Application.Resources;
using Identity.Domain.Enum;
using Microsoft.Extensions.Caching.Memory;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.Interfaces;

namespace Identity.Application.Features.Users.Commands.Queries;

public sealed class GetUsersRequestHandler(
    IUserRepository userRepository,
    IMemoryCache memoryCache) : IRequestHandler<GetUsersRequest, CollectionResult<GetUsersDto>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<CollectionResult<GetUsersDto>> Handle(GetUsersRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out List<GetUsersDto>? usersWithRoles))
            {
                return new CollectionResult<GetUsersDto>
                {
                    Data = usersWithRoles,
                    Count = usersWithRoles!.Count,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage = SuccessMessage.UsersSuccessfullyGeted,
                };
            }

            var users = await userRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);

            if (users.Count == 0)
            {
                return new CollectionResult<GetUsersDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture) ?? string.Empty],
                };
            }

            var roles = users.Select(user => userRepository.GetUserRolesAsync(user, cancellationToken)).ToList();

            var getUsersDto = users.Select(key => new GetUsersDto
            (
                UserId: key.Id,
                FirstName: key.FirstName,
                LastName: key.LastName,
                UserName: key.UserName!,
                EmailAddress: key.Email!,
                Role: roles,
                CreatedTime: key.CreatedTime,
                ModifiedTime: key.ModifiedTime
            )).OrderByDescending(key => key.CreatedTime).ToList();

            memoryCache.Remove(CacheKey);

            return new CollectionResult<GetUsersDto>
            {
                Data = getUsersDto,
                Count = getUsersDto.Count,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UsersSuccessfullyGeted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<GetUsersDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}