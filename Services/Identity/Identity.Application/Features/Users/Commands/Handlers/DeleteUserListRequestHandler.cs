using FluentValidation;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;


namespace Identity.Application.Features.Users.Commands.Handlers
{
    public class DeleteUserListRequestHandler(UserManager<ApplicationUser> userManager, IDeleteUserListDtoValidator deleteValidator, ILogger logger, IMemoryCache memoryCache, ApplicationDbContext context)
        : IRequestHandler<DeleteUserListRequest, Result<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<DeleteUserListRequestHandler>();
        private readonly IDeleteUserListDtoValidator _deleteValidator = deleteValidator;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly ApplicationDbContext _context = context;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<Result<bool>> Handle(DeleteUserListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteUserList, cancellationToken);

                if (!validator.IsValid)
                {
                    var existErrorMessage = new Dictionary<string, List<string>>
                    {
                       {"UserIds", validator.Errors.Select(key => key.ErrorMessage).ToList() }
                    };

                    foreach (var error in existErrorMessage)
                    {
                        if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<bool>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.UsersConNotDeleted,
                                ErrorCode = (int)ErrorCodes.UsersConNotDeleted,
                            };
                        }
                    }

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
                            ValidationErrors = [ErrorMessage.UsersNotFound]
                        };
                    }

                    else
                    {
                        foreach (var user in users)
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
                                return new Result<bool>
                                {
                                    ErrorMessage = ErrorMessage.UsersConNotDeleted,
                                    ErrorCode = (int)ErrorCodes.UsersConNotDeleted,
                                    ValidationErrors = [ErrorMessage.UsersConNotDeleted]
                                };
                            }
                        }
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    _memoryCache.Remove(users);

                    var usersCache = await _userManager.Users.ToListAsync(cancellationToken);
                    _memoryCache.Set(cacheKey, usersCache);

                    foreach (var user in users)
                    {
                        _memoryCache.Set(cacheKey, user);
                    }

                    return new Result<bool>
                    {
                        Data = true,
                        SuccessCode = (int)SuccessCode.Deleted,
                        SuccessMessage = SuccessMessage.UsersSuccessfullyDeleted,
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
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}