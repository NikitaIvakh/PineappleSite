using Identity.Application.Features.Users.Requests.Queries;
using Identity.Application.Resources;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Queries
{
    public class GetUserForUpdateRequestHandler(UserManager<ApplicationUser> userManager, IMemoryCache memoryCache) : IRequestHandler<GetUserForUpdateRequest, Result<GetUserForUpdateDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<Result<GetUserForUpdateDto>> Handle(GetUserForUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out GetUserForUpdateDto? resultUser))
                {
                    return new Result<GetUserForUpdateDto>
                    {
                        Data = resultUser,
                        SuccessCode = (int)SuccessCode.Ok,
                        SuccessMessage = SuccessMessage.UserSuccessfullyGeted,
                    };
                }

                else
                {
                    var user = await _userManager.FindByIdAsync(request.UserId);

                    if (user is null)
                    {
                        return new Result<GetUserForUpdateDto>
                        {
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ValidationErrors = [ErrorMessage.UserNotFound]
                        };
                    }

                    else
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        resultUser = new GetUserForUpdateDto(user.Id, user.FirstName, user.LastName, user.UserName!, user.Email!, roles, user.Description!, user.Age, request.Password!, user.ImageUrl!, user.ImageLocalPath!);

                        _memoryCache.Remove(resultUser);
                        _memoryCache.Set(cacheKey, resultUser);

                        return new Result<GetUserForUpdateDto>
                        {
                            Data = resultUser,
                            SuccessCode = (int)SuccessCode.Ok,
                            SuccessMessage = SuccessMessage.UserSuccessfullyGeted,
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                return new Result<GetUserForUpdateDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}