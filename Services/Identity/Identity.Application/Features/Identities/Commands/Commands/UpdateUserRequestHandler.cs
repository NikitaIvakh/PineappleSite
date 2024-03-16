using Identity.Application.Extecsions;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class UpdateUserRequestHandler(UserManager<ApplicationUser> userManager, IUpdateUserRequestDtoValidator validationRules, ILogger logger) : IRequestHandler<UpdateUserRequest, Result<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUpdateUserRequestDtoValidator _updateValidator = validationRules;
        private readonly ILogger _logger = logger.ForContext<UpdateUserRequest>();

        public async Task<Result<RegisterResponseDto>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _updateValidator.ValidateAsync(request.UpdateUser, cancellationToken);

                if (!validator.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>
                    {
                        {"FirstName",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"LastName",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"UserName",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"EmailAddress",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var message))
                        {
                            return new Result<RegisterResponseDto>
                            {
                                ValidationErrors = message,
                                ErrorMessage = ErrorMessage.UserUpdateError,
                                ErrorCode = (int)ErrorCodes.UserUpdateError,
                            };
                        }
                    }

                    return new Result<RegisterResponseDto>
                    {
                        ErrorMessage = ErrorMessage.UserUpdateError,
                        ErrorCode = (int)ErrorCodes.UserUpdateError,
                        ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var user = await _userManager.FindByIdAsync(request.UpdateUser.Id);

                    if (user is null)
                    {
                        return new Result<RegisterResponseDto>
                        {
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                        };
                    }

                    else
                    {
                        user.FirstName = request.UpdateUser.FirstName;
                        user.LastName = request.UpdateUser.LastName;
                        user.Email = request.UpdateUser.EmailAddress;
                        user.UserName = request.UpdateUser.UserName;

                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            var existsRoles = await _userManager.GetRolesAsync(user);
                            await _userManager.RemoveFromRolesAsync(user, existsRoles);


                            return new Result<RegisterResponseDto>
                            {
                                Data = new RegisterResponseDto { UserId = user.Id },
                                SuccessMessage = "Успешное обновление пользователя",
                            };
                        }

                        else
                        {
                            return new Result<RegisterResponseDto>
                            {
                                ErrorMessage = ErrorMessage.UserUpdateError,
                                ErrorCode = (int)ErrorCodes.UserUpdateError,
                            };
                        }
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<RegisterResponseDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}