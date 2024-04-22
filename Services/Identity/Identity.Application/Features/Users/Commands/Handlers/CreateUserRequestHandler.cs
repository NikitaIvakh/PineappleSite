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

public sealed class CreateUserRequestHandler(
    UserManager<ApplicationUser> userManager,
    CreateUserValidation createValidator,
    IMemoryCache memoryCache,
    ApplicationDbContext context) : IRequestHandler<CreateUserRequest, Result<string>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<string>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = await createValidator.ValidateAsync(request.CreateUser, cancellationToken);

            if (!validator.IsValid)
            {
                var existsErrors = new Dictionary<string, List<string>>()
                {
                    { "FirstName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "LastName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "UserName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "EmailAddress", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "Password", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "Roles", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                };

                foreach (var error in existsErrors)
                {
                    if (existsErrors.TryGetValue(error.Key, out var errorMessages))
                    {
                        return new Result<string>
                        {
                            ValidationErrors = errorMessages,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("UserCanNotBeCreated", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserCanNotBeCreated", ErrorMessage.Culture),
                };
            }

            var userExists = await userManager.FindByNameAsync(request.CreateUser.UserName);

            if (userExists is not null)
            {
                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserAlreadyExists", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("UserAlreadyExists", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var existsEmail = await userManager.FindByEmailAsync(request.CreateUser.EmailAddress);

            if (existsEmail is not null)
            {
                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("EmailAddressAlreadyExists", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("EmailAddressAlreadyExists", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.CreateUser.FirstName.Trim(),
                LastName = request.CreateUser.LastName.Trim(),
                Email = request.CreateUser.EmailAddress.Trim(),
                UserName = request.CreateUser.UserName.Trim(),
                EmailConfirmed = true,
            };

            if (!string.IsNullOrEmpty(request.CreateUser.Password))
            {
                var passwordHasher =
                    userManager.PasswordHasher.HashPassword(user, request.CreateUser.Password);
                user.PasswordHash = passwordHasher;
            }

            var result = await userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, request.CreateUser.Roles.ToString());
                await userManager.UpdateAsync(user);
                await context.SaveChangesAsync(cancellationToken);

                memoryCache.Remove(CacheKey);

                return new Result<string>
                {
                    Data = user.Id,
                    StatusCode = (int)StatusCode.Created,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("UserSuccessfullyCreated", SuccessMessage.Culture),
                };
            }

            memoryCache.Remove(CacheKey);

            return new Result<string>
            {
                StatusCode = (int)StatusCode.NoAction,
                ErrorMessage = ErrorMessage.ResourceManager.GetString("UserCanNotBeCreated", ErrorMessage.Culture),
                ValidationErrors =
                [
                    ErrorMessage.ResourceManager.GetString("UserCanNotBeCreated", ErrorMessage.Culture) ??
                    string.Empty
                ]
            };
        }

        catch (Exception ex)
        {
            return new Result<string>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}