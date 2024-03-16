using FluentValidation;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Identity.Application.Features.Users.Commands.Handlers
{
    public class DeleteUserListRequestHandler(UserManager<ApplicationUser> userManager, IDeleteUserListDtoValidator deleteValidator, ILogger logger) : IRequestHandler<DeleteUserListRequest, Result<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<DeleteUserListRequestHandler>();
        private readonly IDeleteUserListDtoValidator _deleteValidator = deleteValidator;

        public async Task<Result<bool>> Handle(DeleteUserListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteUserList, cancellationToken);

                if (!validator.IsValid)
                {
                    return new Result<bool>
                    {
                        ErrorMessage = ErrorMessage.UsersConNotDeleted,
                        ErrorCode = (int)ErrorCodes.UsersConNotDeleted,
                        ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var users = await _userManager.Users.Where(key => request.DeleteUserList.UserIds.Contains(key.Id)).ToListAsync(cancellationToken);

                    if (users is null || users.Count == 0)
                    {
                        return new Result<bool>
                        {
                            ErrorMessage = ErrorMessage.UsersNotFound,
                            ErrorCode = (int)ErrorCodes.UsersNotFound,
                        };
                    }

                    else
                    {
                        foreach (var user in users)
                        {
                            var result = await _userManager.DeleteAsync(user);

                            if (!result.Succeeded)
                            {
                                return new Result<bool>
                                {
                                    ErrorMessage = ErrorMessage.UsersConNotDeleted,
                                    ErrorCode = (int)ErrorCodes.UsersConNotDeleted,
                                };
                            }
                        }
                    }

                    return new Result<bool>
                    {
                        Data = true,
                        SuccessMessage = "Пользователи успешно удалены",
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<bool>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}