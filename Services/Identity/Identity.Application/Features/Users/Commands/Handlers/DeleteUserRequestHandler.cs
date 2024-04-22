using MediatR;
using Identity.Domain.Enum;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.ResultIdentity;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Handlers;

public sealed class DeleteUserRequestHandler(
    IUserRepository userRepository,
    DeleteUserValidator deleteValidator,
    IMemoryCache memoryCache) : IRequestHandler<DeleteUserRequest, Result<Unit>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<Unit>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = await deleteValidator.ValidateAsync(request.DeleteUser, cancellationToken);

            if (!validator.IsValid)
            {
                var existErrorMessage = new Dictionary<string, List<string>>
                {
                    { "UserId", validator.Errors.Select(key => key.ErrorMessage).ToList() }
                };

                foreach (var error in existErrorMessage)
                {
                    if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new Result<Unit>
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("UserCanNotDeleted", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserCanNotDeleted", ErrorMessage.Culture),
                };
            }

            var user = await userRepository.GetAll(cancellationToken)
                .FirstOrDefaultAsync(key => key.Id == request.DeleteUser.UserId, cancellationToken);

            if (user is null)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty],
                };
            }

            if (!string.IsNullOrEmpty(user.ImageLocalPath))
            {
                var fileName = $"Id_{user.Id}*";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");

                var getAllFiles = Directory.GetFiles(filePath, fileName + ".*");

                foreach (var file in getAllFiles)
                {
                    File.Delete(file);
                }

                user.ImageUrl = null;
                user.ImageLocalPath = null;

                await userRepository.UpdateUserAsync(user, cancellationToken);
            }

            var result = await userRepository.DeleteUserAsync(user, cancellationToken);

            if (!result.Succeeded)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserCanNotDeleted", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UserCanNotDeleted", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UserSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}