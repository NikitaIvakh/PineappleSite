using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class UpdateUserProfileRequestHandler(UserManager<ApplicationUser> userManager, IUpdateUserProfileDto updateUserProfileDto, IHttpContextAccessor httpContextAccessor) :
        IRequestHandler<UpdateUserProfileRequest, BaseIdentityResponse<UserWithRoles>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUpdateUserProfileDto _updateUserProfileDto = updateUserProfileDto;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<BaseIdentityResponse<UserWithRoles>> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<UserWithRoles>();

            try
            {
                var validator = await _updateUserProfileDto.ValidateAsync(request.UpdateUserProfile, cancellationToken);

                if (!validator.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка обновления профиля";
                    response.ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList();
                }

                else
                {
                    var user = await _userManager.FindByIdAsync(request.UpdateUserProfile.Id) ??
                        throw new Exception($"Пользователь ({request.UpdateUserProfile.UserName}) не найден");

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
                        var userWithRoles = new UserWithRoles
                        {
                            User = user,
                            Roles = existsRoles,
                        };

                        response.IsSuccess = true;
                        response.Message = "Профиль пользователя успешно обновлен";
                        response.Data = userWithRoles;
                    }
                }

                return response;
            }

            catch (Exception exception)
            {
                response.IsSuccess = false;
                response.Message = exception.Message;
            }

            return response;
        }
    }
}