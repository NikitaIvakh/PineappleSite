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
using Serilog;

namespace Identity.Application.Features.Users.Commands.Handlers
{
    public class UpdateUserRequestHandler(UserManager<ApplicationUser> userManager, IUpdateUserRequestDtoValidator validationRules, ILogger logger, IMemoryCache memoryCache)
        : IRequestHandler<UpdateUserRequest, Result<Unit>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUpdateUserRequestDtoValidator _updateValidator = validationRules;
        private readonly ILogger _logger = logger.ForContext<UpdateUserRequest>();
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<Result<Unit>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
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
                            return new Result<Unit>
                            {
                                ValidationErrors = message,
                                ErrorMessage = ErrorMessage.UserUpdateError,
                                ErrorCode = (int)ErrorCodes.UserUpdateError,
                            };
                        }
                    }

                    return new Result<Unit>
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
                        return new Result<Unit>
                        {
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                        };
                    }

                    else
                    {
                        user.FirstName = request.UpdateUser.FirstName.Trim();
                        user.LastName = request.UpdateUser.LastName.Trim();
                        user.Email = request.UpdateUser.EmailAddress.Trim();
                        user.UserName = request.UpdateUser.UserName.Trim();

                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            var existsRoles = await _userManager.GetRolesAsync(user);
                            await _userManager.RemoveFromRolesAsync(user, existsRoles);

                            await _userManager.AddToRoleAsync(user, request.UpdateUser.UserRoles.ToString());
                            await _userManager.UpdateAsync(user);

                            _memoryCache.Remove(user);

                            var usersCache1 = await _userManager.Users.ToListAsync(cancellationToken);
                            _memoryCache.Set(cacheKey, usersCache1);
                            _memoryCache.Set(cacheKey, user);

                            return new Result<Unit>
                            {
                                Data = Unit.Value,
                                SuccessCode = (int)SuccessCode.Updated,
                                SuccessMessage = SuccessMessage.UserSuccessfullyUpdated,
                            };
                        }

                        else
                        {
                            return new Result<Unit>
                            {
                                ErrorMessage = ErrorMessage.UserUpdateError,
                                ErrorCode = (int)ErrorCodes.UserUpdateError,
                                ValidationErrors = [ErrorMessage.UserUpdateError]
                            };
                        }
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<Unit>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}