using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.Interfaces;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Handlers;

public sealed class UpdateUserProfileRequestHandler(
    IUserRepository userRepository,
    UpdateUserProfileValidator userProfileValidator,
    IHttpContextAccessor httpContextAccessor,
    IMemoryCache memoryCache) : IRequestHandler<UpdateUserProfileRequest, Result<Unit>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<Unit>> Handle(UpdateUserProfileRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validator = await userProfileValidator.ValidateAsync(request.UpdateUserProfile, cancellationToken);

            if (!validator.IsValid)
            {
                var errorMessages = new Dictionary<string, List<string>>
                {
                    { "FirstName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "LastName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "UserName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "EmailAddress", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "Description", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "Age", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "Password", validator.Errors.Select(x => x.ErrorMessage).ToList() },
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
                                ErrorMessage.ResourceManager.GetString("UpdateProfileError", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UpdateProfileError", ErrorMessage.Culture),
                };
            }

            var user = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.Id == request.UpdateUserProfile.Id, cancellationToken);

            if (user is null)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            user.FirstName = request.UpdateUserProfile.FirstName!.Trim();
            user.LastName = request.UpdateUserProfile.LastName!.Trim();
            user.UserName = request.UpdateUserProfile.UserName!.Trim();
            user.Email = request.UpdateUserProfile.EmailAddress!.Trim();
            user.Description = request.UpdateUserProfile.Description!.Trim();
            user.Age = request.UpdateUserProfile.Age;

            if (!string.IsNullOrEmpty(request.UpdateUserProfile.Password))
            {
                var newPassword =
                    new PasswordHasher<ApplicationUser>().HashPassword(user, request.UpdateUserProfile.Password);

                user.PasswordHash = newPassword;
            }

            await userRepository.UpdateUserAsync(user, cancellationToken);

            if (request.UpdateUserProfile.Avatar is not null)
            {
                if (!string.IsNullOrEmpty(user.ImageLocalPath))
                {
                    var fileNameFromDatabase = $"Id_{user.Id}*";
                    var filePathPromDatabase =
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");

                    var files = Directory.GetFiles(filePathPromDatabase, fileNameFromDatabase + ".*");

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    user.ImageUrl = null;
                    user.ImageLocalPath = null;

                    await userRepository.UpdateUserAsync(user, cancellationToken);
                }

                var fileName = $"Id_{user.Id}------{Guid.NewGuid()}" +
                               Path.GetExtension(request.UpdateUserProfile.Avatar.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");
                var directory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var fileFullPath = Path.Combine(directory, fileName);

                await using (FileStream fileStream = new(fileFullPath, FileMode.Create))
                {
                    await request.UpdateUserProfile.Avatar.CopyToAsync(fileStream, cancellationToken);
                }

                var baseUrl =
                    $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}{httpContextAccessor.HttpContext.Request.PathBase.Value}";

                user.ImageUrl = Path.Combine(baseUrl, "UserImages", fileName);
                user.ImageLocalPath = filePath;
            }

            else
            {
                request.UpdateUserProfile = request.UpdateUserProfile with { ImageUrl = user.ImageUrl };
                request.UpdateUserProfile = request.UpdateUserProfile with { ImageLocalPath = user.ImageLocalPath };
            }

            var result = await userRepository.UpdateUserAsync(user, cancellationToken);

            if (!result.Succeeded)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserCanNotBeUpdated", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UserCanNotBeUpdated", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Modify,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UserProfileSuccessfullyUpdated", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }
}