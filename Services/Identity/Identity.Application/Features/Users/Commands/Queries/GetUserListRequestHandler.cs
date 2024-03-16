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
using Identity.Domain.DTOs.Authentications;
using Identity.Application.Features.Users.Requests.Queries;

namespace Identity.Application.Features.Users.Commands.Queries
{
    public class GetUserListRequestHandler(UserManager<ApplicationUser> userManager, ILogger logger, IMemoryCache memoryCache) : IRequestHandler<GetUserListRequest, CollectionResult<UserWithRolesDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<GetUserListRequest>();
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheUserListKey";

        public async Task<CollectionResult<UserWithRolesDto>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out List<UserWithRolesDto>? usersWithRoles))
                {
                    return new CollectionResult<UserWithRolesDto>
                    {
                        Data = usersWithRoles,
                        Count = usersWithRoles!.Count
                    };
                }

                usersWithRoles = [];
                var currentUser = await _userManager.FindByIdAsync(request.UserId);

                if (currentUser is not null)
                {
                    var roles = await _userManager.GetRolesAsync(currentUser);

                    if (roles.Contains(RoleConsts.Administrator))
                    {
                        var users = await _userManager.Users.ToListAsync(cancellationToken);

                        if (users is null || users.Count == 0)
                        {
                            return new CollectionResult<UserWithRolesDto>
                            {
                                ErrorMessage = ErrorMessage.UsersNotFound,
                                ErrorCode = (int)ErrorCodes.UsersNotFound,
                            };
                        }

                        else
                        {
                            foreach (var user in users)
                            {
                                var role = await _userManager.GetRolesAsync(user);
                                usersWithRoles.Add(new UserWithRolesDto { User = user, Roles = role });
                            }
                        }
                    }

                    else
                    {
                        var userWithRoles = new UserWithRolesDto
                        {
                            User = currentUser,
                            Roles = roles.ToList()
                        };

                        usersWithRoles.Add(userWithRoles);
                    }
                }

                return new CollectionResult<UserWithRolesDto>
                {
                    Data = usersWithRoles,
                    Count = usersWithRoles.Count
                };
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                _memoryCache.Remove(cacheKey);
                return new CollectionResult<UserWithRolesDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }
        }
    }
}