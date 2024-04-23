using Identity.Application.Features.Users.Requests.Queries;
using Identity.Application.Resources;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Enum;
using Identity.Domain.Interfaces;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Queries;

public sealed class GetUserForUpdateRequestHandler(
    IUserRepository userRepository,
    IMemoryCache memoryCache) : IRequestHandler<GetUserForUpdateRequest, Result<GetUserForUpdateDto>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<GetUserForUpdateDto>> Handle(GetUserForUpdateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out GetUserForUpdateDto? resultUser))
            {
                return new Result<GetUserForUpdateDto>
                {
                    Data = resultUser,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("UserSuccessfullyGot", SuccessMessage.Culture),
                };
            }

            var user = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return new Result<GetUserForUpdateDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty
                    ]
                };
            }

            var roles = await userRepository.GetUserRolesAsync(user);
            var getUserFotUpdate = new GetUserForUpdateDto
            (
                UserId: user.Id,
                FirstName: user.FirstName,
                LastName: user.LastName,
                UserName: user.UserName!,
                EmailAddress: user.Email!,
                Role: roles,
                Description: user.Description!,
                Age: user.Age,
                Password: request.Password!,
                ImageUrl: user.ImageUrl!,
                ImageLocalPath: user.ImageLocalPath!);

            memoryCache.Remove(CacheKey);

            return new Result<GetUserForUpdateDto>
            {
                Data = getUserFotUpdate,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UserSuccessfullyGot", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<GetUserForUpdateDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}