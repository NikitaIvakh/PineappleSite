using AutoMapper;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Application.Resources;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Identity.Application.Features.Identities.Commands.Queries
{
    public class GetUserDetailsRequestHandler(UserManager<ApplicationUser> userManager, ILogger logger, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<GetUserDetailsRequest, Result<UserWithRolesDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<GetUserDetailsRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheGetUserKey";

        public async Task<Result<UserWithRolesDto>> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out UserWithRoles? userWithRoles))
                {
                    return new Result<UserWithRolesDto>
                    {
                        Data = _mapper.Map<UserWithRolesDto>(userWithRoles),
                    };
                }

                var user = await _userManager.FindByIdAsync(request.Id);

                if (user is null)
                {
                    return new Result<UserWithRolesDto>
                    {
                        ErrorMessage = ErrorMessage.UserNotFound,
                        ErrorCode = (int)ErrorCodes.UserNotFound,
                    };
                }

                else
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userWithRoles = new UserWithRoles
                    {
                        User = user,
                        Roles = roles.ToList()
                    };

                    return new Result<UserWithRolesDto>
                    {
                        Data = _mapper.Map<UserWithRolesDto>(userWithRoles),
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                _memoryCache.Remove(cacheKey);
                return new Result<UserWithRolesDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}