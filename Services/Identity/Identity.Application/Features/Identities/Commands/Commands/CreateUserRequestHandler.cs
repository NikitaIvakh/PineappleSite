using Identity.Application.Extecsions;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class CreateUserRequestHandler(UserManager<ApplicationUser> userManager, ICreateUserDtoValidation createValidator) : IRequestHandler<CreateUserRequest, Result<ApplicationUser>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICreateUserDtoValidation _createValidator = createValidator;

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
                                FirstName = request.CreateUser.FirstName,
                                LastName = request.CreateUser.LastName,
                                Email = request.CreateUser.EmailAddress,
                                UserName = request.CreateUser.UserName,
                            };

                            if (!string.IsNullOrEmpty(request.CreateUser.Password))
                            {
                                var passwordHasher = _userManager.PasswordHasher.HashPassword(user, request.CreateUser.Password);
                                user.PasswordHash = passwordHasher;
                            }

                            var result = await _userManager.CreateAsync(user);

                            if (result.Succeeded)
                            {
                                var userRoles = await _userManager.GetRolesAsync(user);
                                await _userManager.RemoveFromRolesAsync(user, userRoles);

                                var roleName = request.CreateUser.Roles.GetDisplayName();
                                await _userManager.AddToRoleAsync(user, roleName);

                                await _userManager.UpdateAsync(user);

                                return new Result<ApplicationUser>
                                {
                                    Data = user,
                                    SuccessMessage = "Пользователь успешно создан",
                                };
                            }

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