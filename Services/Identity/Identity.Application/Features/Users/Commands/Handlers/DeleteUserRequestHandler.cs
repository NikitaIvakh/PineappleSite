using Serilog;
using MediatR;
using Identity.Domain.Enum;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.ResultIdentity;
using Microsoft.AspNetCore.Identity;
using Identity.Application.Features.Users.Requests.Handlers;

namespace Identity.Application.Features.Users.Commands.Handlers
{
    public class DeleteUserRequestHandler(UserManager<ApplicationUser> userManager, IDeleteUserDtoValidator deleteValidator, ILogger logger) : IRequestHandler<DeleteUserRequest, Result<DeleteUserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<DeleteUserRequestHandler>();
        private readonly IDeleteUserDtoValidator _deleteValidator = deleteValidator;

        public async Task<Result<DeleteUserDto>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteUser, cancellationToken);

                if (!validator.IsValid)
                {
                    return new Result<DeleteUserDto>
                    {
                        ErrorMessage = ErrorMessage.UserCanNotDeleted,
                        ErrorCode = (int)ErrorCodes.UserCanNotDeleted,
                        ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var user = await _userManager.FindByIdAsync(request.DeleteUser.Id);

                    if (user is null)
                    {
                        return new Result<DeleteUserDto>
                        {
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                        };
                    }

                    else
                    {
                        var result = await _userManager.DeleteAsync(user);

                        if (!result.Succeeded)
                        {
                            return new Result<DeleteUserDto>
                            {
                                ErrorMessage = ErrorMessage.UserCanNotDeleted,
                                ErrorCode = (int)ErrorCodes.UserCanNotDeleted,
                            };
                        }

                        else
                        {
                            return new Result<DeleteUserDto>
                            {
                                Data = request.DeleteUser,
                                SuccessMessage = "Пользователь успешно удален",
                            };
                        }
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<DeleteUserDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}