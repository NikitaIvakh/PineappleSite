using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Handlers;

public sealed class DeleteUsersRequestHandler(
    UserManager<ApplicationUser> userManager,
    DeleteUsersValidator deleteValidator,
    IMemoryCache memoryCache) : IRequestHandler<DeleteUsersRequest, CollectionResult<Unit>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<CollectionResult<Unit>> Handle(DeleteUsersRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = await deleteValidator.ValidateAsync(request.DeleteUsers, cancellationToken);

            if (!validator.IsValid)
            {
                var existErrorMessage = new Dictionary<string, List<string>>
                {
                    { "UserIds", validator.Errors.Select(key => key.ErrorMessage).ToList() }
                };

                foreach (var error in existErrorMessage)
                {
                    if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new CollectionResult<Unit>
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NotFound,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("UsersConNotDeleted", ErrorMessage.Culture),
                        };
                    }
                }

                return new CollectionResult<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("UsersConNotDeleted", ErrorMessage.Culture),
                };
            }

            var users = await userManager.Users
                .Where(key => request.DeleteUsers.UserIds.Contains(key.Id))
                .ToListAsync(cancellationToken);

            if (users.Count == 0)
            {
                return new CollectionResult<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            foreach (var user in users)
            {
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

                    await userManager.UpdateAsync(user);
                }

                var result = await userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return new CollectionResult<Unit>
                    {
                        StatusCode = (int)StatusCode.NoAction,
                        ErrorMessage =
                            ErrorMessage.ResourceManager.GetString("UsersConNotDeleted", ErrorMessage.Culture),
                        ValidationErrors =
                        [
                            ErrorMessage.ResourceManager.GetString("UsersConNotDeleted", ErrorMessage.Culture) ??
                            string.Empty
                        ]
                    };
                }
            }

            memoryCache.Remove(CacheKey);

            return new CollectionResult<Unit>
            {
                Data = [Unit.Value],
                Count = users.Count,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UsersSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}