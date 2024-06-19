using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Handlers;

public sealed class UpdateUserProfileRequestHandler(
    UserManager<ApplicationUser> userManager,
    UpdateUserProfileValidator userProfileValidator,
    IHttpContextAccessor httpContextAccessor,
    IMemoryCache memoryCache) : IRequestHandler<UpdateUserProfileRequest, Result<GetUserForUpdateDto>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<GetUserForUpdateDto>> Handle(UpdateUserProfileRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validator = await userProfileValidator.ValidateAsync(request.UpdateUserProfile, cancellationToken);

            if (!validator.IsValid)
            {
                var errorMessages = new Dictionary<string, List<string>>
                {
                    { "Id", validator.Errors.Select(x => x.ErrorMessage).ToList() },
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
                        return new Result<GetUserForUpdateDto>
                        {
                            ValidationErrors = message,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("UpdateProfileError", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<GetUserForUpdateDto>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UpdateProfileError", ErrorMessage.Culture),
                };
            }

            var user = await userManager.FindByIdAsync(request.UpdateUserProfile.Id);

            if (user is null)
            {
                return new Result<GetUserForUpdateDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            user.FirstName = request.UpdateUserProfile.FirstName?.Trim() ?? string.Empty;
            user.LastName = request.UpdateUserProfile.LastName?.Trim() ?? string.Empty;
            user.UserName = request.UpdateUserProfile.UserName?.Trim() ?? string.Empty;
            user.Description = request.UpdateUserProfile.Description?.Trim() ?? string.Empty;
            user.Email = request.UpdateUserProfile.EmailAddress?.Trim().ToLower() ?? string.Empty;
            user.Age = request.UpdateUserProfile?.Age;

            if (!string.IsNullOrEmpty(request.UpdateUserProfile?.Password))
            {
                var newPassword =
                    new PasswordHasher<ApplicationUser>().HashPassword(user, request.UpdateUserProfile.Password);

                user.PasswordHash = newPassword;
            }

            await userManager.UpdateAsync(user);

            if (request.UpdateUserProfile?.Avatar is not null)
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

                    await userManager.UpdateAsync(user);
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

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new Result<GetUserForUpdateDto>
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

            var roles = await userManager.GetRolesAsync(user);
            var getUser = new GetUserForUpdateDto
            (
                UserId: user.Id,
                FirstName: user.FirstName,
                LastName: user.LastName,
                UserName: user.UserName,
                EmailAddress: user.Email.ToLower(),
                Role: roles,
                Description: user.Description,
                Age: user.Age,
                Password: request.UpdateUserProfile.Password,
                ImageUrl: user?.ImageUrl,
                ImageLocalPath: user?.ImageLocalPath
            );

            memoryCache.Remove(CacheKey);

            return new Result<GetUserForUpdateDto>
            {
                Data = getUser,
                StatusCode = (int)StatusCode.Modify,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UserProfileSuccessfullyUpdated", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<GetUserForUpdateDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }
}