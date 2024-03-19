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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Users.Commands.Handlers
{
    public class DeleteUserRequestHandler(UserManager<ApplicationUser> userManager, IDeleteUserDtoValidator deleteValidator, ILogger logger, IMemoryCache memoryCache) : IRequestHandler<DeleteUserRequest, Result<DeleteUserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<DeleteUserRequestHandler>();
        private readonly IDeleteUserDtoValidator _deleteValidator = deleteValidator;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<Result<DeleteUserDto>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteUser, cancellationToken);

                if (!validator.IsValid)
                {
                    var existErrorMessage = new Dictionary<string, List<string>>
                    {
                        {"Id", validator.Errors.Select(key => key.ErrorMessage).ToList() }
                    };

                    foreach (var error in existErrorMessage)
                    {
                        if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<DeleteUserDto>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.UserCanNotDeleted,
                                ErrorCode = (int)ErrorCodes.UserCanNotDeleted,
                            };
                        }
                    }

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
                            ValidationErrors = [ErrorMessage.UserNotFound]
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
                                ValidationErrors = [ErrorMessage.UserCanNotDeleted]
                            };
                        }

                        else
                        {
                            _memoryCache.Remove(user);

                            var users = await _userManager.Users.ToListAsync(cancellationToken);
                            _memoryCache.Set(cacheKey, users);
                            _memoryCache.Set(cacheKey, user);

                            return new Result<DeleteUserDto>
                            {
                                Data = request.DeleteUser,
                                SuccessCode = (int)SuccessCode.Deleted,
                                SuccessMessage = SuccessMessage.UserSuccessfullyDeleted,
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
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}