using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.Interfaces;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Application.Features.Users.Commands.Handlers;

public sealed class CreateUserRequestHandler(
    IUserRepository userRepository,
    CreateUserValidation createValidator,
    IMemoryCache memoryCache) : IRequestHandler<CreateUserRequest, Result<Unit>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<Unit>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
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
                        return new Result<Unit>
                        {
                            ValidationErrors = errorMessages,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("UserCanNotBeCreated", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserCanNotBeCreated", ErrorMessage.Culture),
                };
            }

            var userExists = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.UserName == request.CreateUser.UserName, cancellationToken);

            if (userExists is not null)
            {
                return new Result<Unit>
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

            var existsEmail = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.Email == request.CreateUser.EmailAddress, cancellationToken);

            if (existsEmail is not null)
            {
                return new Result<Unit>
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
                UserName = request.CreateUser.UserName.Trim(),
                Email = request.CreateUser.EmailAddress.Trim().ToLower(),
                EmailConfirmed = true,
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>().HashPassword(user, request.CreateUser.Password);
            user.PasswordHash = passwordHasher;

            var result = await userRepository.CreateUserAsync(user, request.CreateUser.Password, cancellationToken);

            if (!result.Succeeded)
            {
                return new Result<Unit>
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

            await userRepository.AddUserToRoleAsync(user, request.CreateUser.Roles.ToString(), cancellationToken);
            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Created,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UserSuccessfullyCreated", SuccessMessage.Culture),
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