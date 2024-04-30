﻿using Identity.Application.Features.Identities.Requests.Commands;
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

namespace Identity.Application.Features.Identities.Commands.Commands;

public sealed class RegisterUserRequestHandler(
    IUserRepository userRepository,
    RegisterValidator validationRules,
    IMemoryCache memoryCache) : IRequestHandler<RegisterUserRequest, Result<string>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<Result<string>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = await validationRules.ValidateAsync(request.RegisterRequest, cancellationToken);

            if (!validator.IsValid)
            {
                var errorMessages = new Dictionary<string, List<string>>()
                {
                    { "FirstName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "LastName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "UserName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "EmailAddress", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "Password", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                };

                foreach (var error in errorMessages)
                {
                    if (errorMessages.TryGetValue(error.Key, out var message))
                    {
                        return new Result<string>
                        {
                            ValidationErrors = message,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage = ErrorMessage.ResourceManager.GetString("RegistrationLoginError",
                                ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("RegistrationLoginError", ErrorMessage.Culture),
                };
            }

            var existsEmail = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.Email == request.RegisterRequest.EmailAddress, cancellationToken);

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

            var existsUserName = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.UserName == request.RegisterRequest.UserName, cancellationToken);

            if (existsUserName is not null)
            {
                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("ThisUserNameAlreadyExists", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("ThisUserNameAlreadyExists", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.RegisterRequest.FirstName.Trim(),
                LastName = request.RegisterRequest.LastName.Trim(),
                UserName = request.RegisterRequest.UserName.Trim(),
                Email = request.RegisterRequest.EmailAddress.Trim().ToLower(),
                EmailConfirmed = true,
            };

            var passwordHasher =
                new PasswordHasher<ApplicationUser>().HashPassword(user, request.RegisterRequest.Password);

            user.PasswordHash = passwordHasher;

            if (request.RegisterRequest.Password.Trim() != request.RegisterRequest.PasswordConfirm.Trim())
            {
                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("PasswordsDoNotMatch", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("PasswordsDoNotMatch", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var result =
                await userRepository.CreateUserAsync(user, request.RegisterRequest.Password, cancellationToken);

            if (!result.Succeeded)
            {
                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("RegistrationLoginError", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("RegistrationLoginError", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var userFromDb = await userRepository.GetUsers()
                .FirstOrDefaultAsync(key => key.Email == request.RegisterRequest.EmailAddress, cancellationToken);

            if (userFromDb is null)
            {
                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UserNotFound", ErrorMessage.Culture) ?? string.Empty]
                };
            }

            await userRepository.AddUserToRoleAsync(user, RoleConst.User, cancellationToken);
            memoryCache.Remove(CacheKey);

            return new Result<string>
            {
                Data = userFromDb.Id,
                StatusCode = (int)StatusCode.Created,
                SuccessMessage = SuccessMessage.ResourceManager.GetString("SuccessfullyRegister", ErrorMessage.Culture),
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