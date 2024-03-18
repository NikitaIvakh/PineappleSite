using Identity.Application.Extecsions;
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

namespace Identity.Application.Features.Users.Commands.Handlers
{
    public class CreateUserRequestHandler(UserManager<ApplicationUser> userManager, ICreateUserDtoValidation createValidator, IMemoryCache memoryCache) : IRequestHandler<CreateUserRequest, Result<ApplicationUser>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICreateUserDtoValidation _createValidator = createValidator;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<Result<ApplicationUser>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _createValidator.ValidateAsync(request.CreateUser, cancellationToken);

                if (!validator.IsValid)
                {
                    var existsErrors = new Dictionary<string, List<string>>()
                    {
                        {"FirstName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"LastName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"UserName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"EmailAddress", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"Password", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"Roles", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    };

                    foreach (var error in existsErrors)
                    {
                        if (existsErrors.TryGetValue(error.Key, out var errorMessages))
                        {
                            return new Result<ApplicationUser>
                            {
                                ValidationErrors = errorMessages,
                                ErrorMessage = ErrorMessage.UserCanNotBeCreated,
                                ErrorCode = (int)ErrorCodes.UserCanNotBeCreated,
                            };
                        }
                    }

                    return new Result<ApplicationUser>
                    {
                        Data = null,
                        ErrorMessage = ErrorMessage.UserCanNotBeCreated,
                        ErrorCode = (int)ErrorCodes.UserCanNotBeCreated,
                    };
                }

                else
                {
                    var userExists = await _userManager.FindByNameAsync(request.CreateUser.UserName);

                    if (userExists is not null)
                    {
                        return new Result<ApplicationUser>
                        {
                            Data = null,
                            ErrorMessage = ErrorMessage.UserAlreadyExists,
                            ErrorCode = (int)ErrorCodes.UserAlreadyExists,
                        };
                    }

                    else
                    {
                        var existsEmail = await _userManager.FindByEmailAsync(request.CreateUser.EmailAddress);

                        if (existsEmail is not null)
                        {
                            return new Result<ApplicationUser>
                            {
                                Data = null,
                                ErrorMessage = ErrorMessage.EmailAddressAlreadyExists,
                                ErrorCode = (int)ErrorCodes.EmailAddressAlreadyExists,
                            };
                        }

                        else
                        {
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
                                var passwordHasher = _userManager.PasswordHasher.HashPassword(user, request.CreateUser.Password);
                                user.PasswordHash = passwordHasher;
                            }

                            var result = await _userManager.CreateAsync(user);

                            if (result.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(user, request.CreateUser.Roles.ToString());
                                await _userManager.UpdateAsync(user);

                                return new Result<ApplicationUser>
                                {
                                    Data = user,
                                    SuccessMessage = "Пользователь успешно создан",
                                };
                            }

                            _memoryCache.Remove(user);

                            var users = await _userManager.Users.ToListAsync(cancellationToken);
                            _memoryCache.Set(cacheKey, users);
                            _memoryCache.Set(cacheKey, user);

                            return new Result<ApplicationUser>
                            {
                                Data = user,
                                ErrorMessage = ErrorMessage.UserCanNotBeCreated,
                                ErrorCode = (int)ErrorCodes.UserCanNotBeCreated,
                            };
                        }
                    }
                }
            }

            catch (Exception exception)
            {
                return new Result<ApplicationUser>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message },
                };
            };
        }
    }
}