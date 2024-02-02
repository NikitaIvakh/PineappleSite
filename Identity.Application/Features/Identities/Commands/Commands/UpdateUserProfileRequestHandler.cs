using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class UpdateUserProfileRequestHandler(UserManager<ApplicationUser> userManager, IUpdateUserProfileDto updateUserProfileDto, IHttpContextAccessor httpContextAccessor, ILogger logger) : IRequestHandler<UpdateUserProfileRequest, Result<UserWithRolesDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUpdateUserProfileDto _updateUserProfileDto = updateUserProfileDto;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger _logger = logger.ForContext<UpdateUserProfileRequestHandler>();

        public async Task<Result<UserWithRolesDto>> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
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
                            return new Result<UserWithRolesDto>
                            {
                                ValidationErrors = message,
                                ErrorMessage = ErrorMessage.UpdateProdileError,
                                ErrorCode = (int)ErrorCodes.UpdateProdileError,
                            };
                        }
                    }

                    return new Result<UserWithRolesDto>
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
                        return new Result<UserWithRolesDto>
                        {
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                        };
                    }

                    else
                    {
                        user.FirstName = request.UpdateUserProfile.FirstName;
                        user.LastName = request.UpdateUserProfile.LastName;
                        user.Email = request.UpdateUserProfile.EmailAddress;
                        user.UserName = request.UpdateUserProfile.UserName;
                        user.Description = request.UpdateUserProfile.Description;
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
                                var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), user.ImageLocalPath);
                                FileInfo fileInfo = new(oldFilePathDirectory);

                                if (fileInfo.Exists)
                                {
                                    fileInfo.Delete();
                                }
                            }

                            Random random = new();
                            int randomNumber = random.Next(1, 120001);

                            string fileName = $"{user.Id}{randomNumber}" + Path.GetExtension(request.UpdateUserProfile.Avatar.FileName);
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

                            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{_httpContextAccessor.HttpContext.Request.PathBase.Value}";
                            user.ImageUrl = baseUrl + "/UserImages/" + fileName;
                            user.ImageLocalPath = filePath;
                        }

                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            var existsRoles = await _userManager.GetRolesAsync(user);
                            var userWithRoles = new UserWithRolesDto
                            {
                                User = user,
                                Roles = existsRoles,
                            };

                            return new Result<UserWithRolesDto>
                            {
                                Data = userWithRoles,
                                SuccessMessage = "Профиль пользователя успешно обновлен",
                            };
                        }
                    }

                    return new Result<UserWithRolesDto>
                    {
                        SuccessMessage = "Профиль пользователя успешно обновлен",
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<UserWithRolesDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}