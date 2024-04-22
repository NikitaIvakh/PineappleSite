using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Handlers;

public sealed class UpdateUserRequestHandler(
    UserManager<ApplicationUser> userManager,
    UpdateUserValidator updateUserValidator,
    IMemoryCache memoryCache,
    ApplicationDbContext context)
    : IRequestHandler<UpdateUserRequest, Result<Unit>>
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

            var user = await userManager.FindByIdAsync(request.UpdateUser.Id);

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

            user.FirstName = request.UpdateUser.FirstName.Trim();
            user.LastName = request.UpdateUser.LastName.Trim();
            user.Email = request.UpdateUser.EmailAddress.Trim();
            user.UserName = request.UpdateUser.UserName.Trim();

            var result = await userManager.UpdateAsync(user);

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

            var existsRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, existsRoles);

            await userManager.AddToRoleAsync(user, request.UpdateUser.UserRoles.ToString());
            await userManager.UpdateAsync(user);
            await context.SaveChangesAsync(cancellationToken);
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