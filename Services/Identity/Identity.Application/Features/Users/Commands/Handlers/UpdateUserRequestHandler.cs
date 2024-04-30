using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.Enum;
using Identity.Domain.Interfaces;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Handlers;

public sealed class UpdateUserRequestHandler(
    IUserRepository userRepository,
    UpdateUserValidator updateUserValidator,
    IMemoryCache memoryCache) : IRequestHandler<UpdateUserRequest, Result<Unit>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<Unit>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = await updateUserValidator.ValidateAsync(request.UpdateUser, cancellationToken);

            if (!validator.IsValid)
            {
                var errorMessages = new Dictionary<string, List<string>>
                {
                    { "Id", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "FirstName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "LastName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "UserName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "EmailAddress", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                };

                foreach (var error in errorMessages)
                {
                    if (errorMessages.TryGetValue(error.Key, out var message))
                    {
                        return new Result<Unit>
                        {
                            ValidationErrors = message,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("UserUpdateError", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserUpdateError", ErrorMessage.Culture),
                };
            }

            var user = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.Id == request.UpdateUser.Id, cancellationToken);

            if (user is null)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var otherUserWithSameName = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key =>
                        key.UserName == request.UpdateUser.UserName &&
                        key.Id != request.UpdateUser.Id,
                    cancellationToken);

            if (otherUserWithSameName is not null)
            {
                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("UserAlreadyExists", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UserAlreadyExists", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var existUserEmail = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key =>
                        key.Email == request.UpdateUser.EmailAddress &&
                        key.Id != request.UpdateUser.Id,
                    cancellationToken);

            if (existUserEmail is not null)
            {
                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("UserAlreadyExists", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UserAlreadyExists", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            user.Id = request.UpdateUser.Id;
            user.FirstName = request.UpdateUser.FirstName.Trim();
            user.LastName = request.UpdateUser.LastName.Trim();
            user.UserName = request.UpdateUser.UserName.Trim();
            user.Email = request.UpdateUser.EmailAddress.Trim().ToLower();

            var result = await userRepository.UpdateUserAsync(user, cancellationToken);

            if (!result.Succeeded)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserUpdateError", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UserUpdateError", ErrorMessage.Culture) ?? string.Empty
                    ]
                };
            }

            await userRepository.RemoveFromRolesAsync(user, cancellationToken);
            await userRepository.AddUserToRoleAsync(user, request.UpdateUser.UserRoles.ToString(), cancellationToken);
            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Modify,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UserSuccessfullyUpdated", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>
            {
                ErrorMessage = ex.Message,
                StatusCode = (int)StatusCode.InternalServerError,
                ValidationErrors = [ex.Message]
            };
        }
    }
}