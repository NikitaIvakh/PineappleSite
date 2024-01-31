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

                            var roleNames = request.UpdateUser.UserRoles.GetDisplayName();
                            await _userManager.AddToRoleAsync(user, roleNames);
                            RegisterResponseDto updateResponse = new()
                            {
                                UserId = user.Id
                            };

                            return new Result<RegisterResponseDto>
                            {
                                Data = updateResponse,
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