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
using Identity.Infrastructure;

namespace Identity.Application.Features.Users.Commands.Handlers
{
    public class DeleteUserRequestHandler(UserManager<ApplicationUser> userManager, IDeleteUserDtoValidator deleteValidator, ILogger logger, IMemoryCache memoryCache, ApplicationDbContext context)
        : IRequestHandler<DeleteUserRequest, Result<Unit>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<DeleteUserRequestHandler>();
        private readonly IDeleteUserDtoValidator _deleteValidator = deleteValidator;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly ApplicationDbContext _context = context;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<Result<Unit>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
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
                            return new Result<Unit>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.UserCanNotDeleted,
                                ErrorCode = (int)ErrorCodes.UserCanNotDeleted,
                            };
                        }
                    }

                    return new Result<Unit>
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
                        return new Result<Unit>
                        {
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                            ValidationErrors = [ErrorMessage.UserNotFound]
                        };
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(user.ImageLocalPath))
                        {
                            string fileName = $"Id_{user.Id}*";
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");

                            var getAllFiels = Directory.GetFiles(filePath, fileName + ".*");

                            foreach (var file in getAllFiels)
                            {
                                File.Delete(file);
                            }

                            user.ImageUrl = null;
                            user.ImageLocalPath = null;

                            await _userManager.UpdateAsync(user);
                        }

                        var result = await _userManager.DeleteAsync(user);

                        if (!result.Succeeded)
                        {
                            return new Result<Unit>
                            {
                                ErrorMessage = ErrorMessage.UserCanNotDeleted,
                                ErrorCode = (int)ErrorCodes.UserCanNotDeleted,
                                ValidationErrors = [ErrorMessage.UserCanNotDeleted]
                            };
                        }

                        else
                        {
                            await _context.SaveChangesAsync(cancellationToken);

                            _memoryCache.Remove(user);
                            var users = await _userManager.Users.ToListAsync(cancellationToken);
                            _memoryCache.Set(cacheKey, users);
                            _memoryCache.Set(cacheKey, user);

                            return new Result<Unit>
                            {
                                Data = Unit.Value,
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