using MediatR;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.ResultIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Identity.Application.Resources;
using Identity.Domain.Enum;
using Microsoft.Extensions.Caching.Memory;
using Identity.Application.Features.Users.Requests.Queries;

namespace Identity.Application.Features.Users.Commands.Queries
{
    public class GetUserListRequestHandler(UserManager<ApplicationUser> userManager, ILogger logger, IMemoryCache memoryCache) : IRequestHandler<GetUserListRequest, CollectionResult<GetAllUsersDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<GetUserListRequest>();
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<CollectionResult<GetAllUsersDto>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out List<GetAllUsersDto>? usersWithRoles))
                {
                    return new CollectionResult<GetAllUsersDto>
                    {
                        Data = usersWithRoles,
                        Count = usersWithRoles!.Count,
                        SuccessCode = (int)SuccessCode.Ok,
                        SuccessMessage = SuccessMessage.UsersSuccessfullyGeted,
                    };
                }

                else
                {
                    usersWithRoles = [];
                    var users = await _userManager.Users.ToListAsync(cancellationToken);

                    if (users is null || users.Count == 0)
                    {
                        return new CollectionResult<GetAllUsersDto>
                        {
                            ErrorCode = (int)ErrorCodes.UsersNotFound,
                            ErrorMessage = ErrorMessage.UsersNotFound,
                            ValidationErrors = [ErrorMessage.UsersNotFound],
                        };
                    }

                    else
                    {
                        foreach (var user in users)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            usersWithRoles.Add(new GetAllUsersDto(user.Id, user.FirstName, user.LastName, user.UserName!, user.Email!, roles, user.CreatedTime, user.ModifiedTime));
                        }

                        var usersCache = await _userManager.Users.ToListAsync(cancellationToken);
                        _memoryCache.Set(cacheKey, usersCache);
                        _memoryCache.Set(cacheKey, usersWithRoles);

                        return new CollectionResult<GetAllUsersDto>
                        {
                            Data = usersWithRoles,
                            Count = usersWithRoles.Count,
                            SuccessCode = (int)SuccessCode.Ok,
                            SuccessMessage = SuccessMessage.UsersSuccessfullyGeted,
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                _memoryCache.Remove(cacheKey);
                return new CollectionResult<GetAllUsersDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}