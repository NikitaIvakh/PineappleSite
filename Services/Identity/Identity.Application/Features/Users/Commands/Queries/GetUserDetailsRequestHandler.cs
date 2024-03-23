using AutoMapper;
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
using Serilog;

namespace Identity.Application.Features.Users.Commands.Queries
{
    public class GetUserDetailsRequestHandler(UserManager<ApplicationUser> userManager, ILogger logger, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<GetUserDetailsRequest, Result<GetUserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<GetUserDetailsRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<Result<GetUserDto>> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out GetUserDto? userWithRoles))
                {
                    return new Result<GetUserDto>
                    {
                        Data = userWithRoles,
                        SuccessCode = (int)SuccessCode.Ok,
                        SuccessMessage = SuccessMessage.UserSuccessfullyGeted,
                    };
                }

                else
                {
                    var user = await _userManager.FindByIdAsync(request.UserId);

                    if (user is null)
                    {
                        return new Result<GetUserDto>
                        {
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                            ValidationErrors = [ErrorMessage.UserNotFound]
                        };
                    }

                    else
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        userWithRoles = new GetUserDto(user.Id, user.FirstName, user.LastName, user.UserName!, user.Email!, roles, user.CreatedTime, user.ModifiedTime);

                        var users = await _userManager.Users.ToListAsync(cancellationToken);
                        _memoryCache.Set(cacheKey, users);

                        return new Result<GetUserDto>
                        {
                            Data = userWithRoles,
                            SuccessCode = (int)SuccessCode.Ok,
                            SuccessMessage = SuccessMessage.UserSuccessfullyGeted,
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                _memoryCache.Remove(cacheKey);
                return new Result<GetUserDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}