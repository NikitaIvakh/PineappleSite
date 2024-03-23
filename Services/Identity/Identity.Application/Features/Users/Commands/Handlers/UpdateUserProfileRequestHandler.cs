using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Identity.Application.Features.Users.Commands.Handlers
{
    public class UpdateUserProfileRequestHandler(UserManager<ApplicationUser> userManager, IUpdateUserProfileDto updateUserProfileDto, IHttpContextAccessor httpContextAccessor, ILogger logger, IMemoryCache memoryCache, ApplicationDbContext context)
        : IRequestHandler<UpdateUserProfileRequest, Result<GetUserForUpdateDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUpdateUserProfileDto _updateUserProfileDto = updateUserProfileDto;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger _logger = logger.ForContext<UpdateUserProfileRequestHandler>();
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly ApplicationDbContext _context = context;

        private readonly string cacheKey = "СacheUserKey";

        public async Task<Result<GetUserForUpdateDto>> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _updateUserProfileDto.ValidateAsync(request.UpdateUserProfile, cancellationToken);

                if (!validator.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>
                    {
                        {"FirstName",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"LastName",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"UserName",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"EmailAddress",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"Description",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"Age",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                        {"Password",  validator.Errors.Select(x => x.ErrorMessage).ToList()},
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var message))
                        {
                            return new Result<GetUserForUpdateDto>
                            {
                                ValidationErrors = message,
                                ErrorMessage = ErrorMessage.UpdateProdileError,
                                ErrorCode = (int)ErrorCodes.UpdateProdileError,
                            };
                        }
                    }

                    return new Result<GetUserForUpdateDto>
                    {
                        ErrorMessage = ErrorMessage.UpdateProdileError,
                        ErrorCode = (int)ErrorCodes.UpdateProdileError,
                        ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var user = await _userManager.FindByIdAsync(request.UpdateUserProfile.Id);

                    if (user is null)
                    {
                        return new Result<GetUserForUpdateDto>
                        {
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                            ValidationErrors = [ErrorMessage.UserNotFound]
                        };
                    }

                    else
                    {
                        user.FirstName = request.UpdateUserProfile.FirstName.Trim();
                        user.LastName = request.UpdateUserProfile.LastName.Trim();
                        user.UserName = request.UpdateUserProfile.UserName.Trim();
                        user.Email = request.UpdateUserProfile.EmailAddress.Trim();
                        user.Description = request.UpdateUserProfile.Description.Trim();
                        user.Age = request.UpdateUserProfile.Age;

                        if (!string.IsNullOrEmpty(request.UpdateUserProfile.Password))
                        {
                            var newPassword = _userManager.PasswordHasher.HashPassword(user, request.UpdateUserProfile.Password);
                            user.PasswordHash = newPassword;
                        }

                        await _userManager.UpdateAsync(user);

                        if (request.UpdateUserProfile.Avatar is not null)
                        {
                            if (!string.IsNullOrEmpty(user.ImageLocalPath))
                            {
                                var fileNameFromDatabase = $"Id_{user.Id}*";
                                var filePathPromDatabase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");

                                var files = Directory.GetFiles(filePathPromDatabase, fileNameFromDatabase + ".*");

                                foreach (var file in files)
                                {
                                    File.Delete(file);
                                }

                                user.ImageUrl = null;
                                user.ImageLocalPath = null;

                                await _userManager.UpdateAsync(user);
                            }

                            string fileName = $"Id_{user.Id}------{Guid.NewGuid()}" + Path.GetExtension(request.UpdateUserProfile.Avatar.FileName);
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");
                            var directory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            var fileFullPath = Path.Combine(directory, fileName);

                            using (FileStream fileStream = new(fileFullPath, FileMode.Create))
                            {
                                request.UpdateUserProfile.Avatar.CopyTo(fileStream);
                            }

                            var baseUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{_httpContextAccessor.HttpContext.Request.PathBase.Value}";
                            user.ImageUrl = Path.Combine(baseUrl, "UserImages", fileName);
                            user.ImageLocalPath = filePath;
                        }

                        else
                        {
                            request.UpdateUserProfile.ImageUrl = user.ImageUrl;
                            request.UpdateUserProfile.ImageLocalPath = user.ImageLocalPath;
                        }

                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            var existsRoles = await _userManager.GetRolesAsync(user);
                            var userWithRoles = new GetUserForUpdateDto(user.Id, user.FirstName, user.LastName, user.UserName, user.Email, existsRoles, user.Description, user.Age,
                                request.UpdateUserProfile.Password!, user.ImageUrl!, user.ImageLocalPath!);

                            _memoryCache.Remove(user);

                            var usersCache = await _userManager.Users.ToListAsync(cancellationToken);
                            _memoryCache.Set(cacheKey, usersCache);
                            _memoryCache.Set(cacheKey, user);

                            await _context.SaveChangesAsync(cancellationToken);

                            return new Result<GetUserForUpdateDto>
                            {
                                Data = userWithRoles,
                                SuccessCode = (int)SuccessCode.Updated,
                                SuccessMessage = SuccessMessage.UserProfileSuccessfullyUpdated,
                            };
                        }

                        else
                        {
                            return new Result<GetUserForUpdateDto>
                            {
                                ErrorCode = (int)ErrorCodes.UserCanNotBeUpdated,
                                ErrorMessage = ErrorMessage.UserCanNotBeUpdated,
                                ValidationErrors = [ErrorMessage.UserCanNotBeUpdated]
                            };
                        }
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<GetUserForUpdateDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}