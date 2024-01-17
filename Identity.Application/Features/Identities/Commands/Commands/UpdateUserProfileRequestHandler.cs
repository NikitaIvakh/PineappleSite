using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class UpdateUserProfileRequestHandler(UserManager<ApplicationUser> userManager, IUpdateUserProfileDto updateUserProfileDto) : IRequestHandler<UpdateUserProfileRequest, BaseIdentityResponse<ApplicationUser>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUpdateUserProfileDto _updateUserProfileDto = updateUserProfileDto;

        public async Task<BaseIdentityResponse<ApplicationUser>> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<ApplicationUser>();

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

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        response.IsSuccess = true;
                        response.Message = "Профиль пользователя успешно обновлен";
                        response.Data = user;
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