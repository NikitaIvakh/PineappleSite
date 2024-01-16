using Identity.Application.DTOs.Authentications;
using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class UpdateUserRequestHandler(UserManager<ApplicationUser> userManager, IRegisterRequestDtoValidator validationRules) : IRequestHandler<UpdateUserRequest, BaseIdentityResponse<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IRegisterRequestDtoValidator _updateValidator = validationRules;

        public async Task<BaseIdentityResponse<RegisterResponseDto>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<RegisterResponseDto>();

            try
            {
                var validator = await _updateValidator.ValidateAsync(request.UpdateUser, cancellationToken);

                if (!validator.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка обновления пользователя";
                    response.ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList();
                }

                else
                {
                    var user = await _userManager.FindByIdAsync(request.UpdateUser.Id) ?? throw new Exception($"Пользователь не найден");

                    user.FirstName = request.UpdateUser.FirstName;
                    user.LastName = request.UpdateUser.LastName;
                    user.Email = request.UpdateUser.EmailAddress;
                    user.UserName = request.UpdateUser.UserName;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        RegisterResponseDto updateResponse = new()
                        { 
                            UserId = user.Id
                        };

                        response.IsSuccess = true;
                        response.Message = "Успешное обновление пользователя";
                        response.Data = updateResponse;

                        return response;
                    }
                    else
                    {
                        throw new Exception($"{result.Errors}");
                    }
                }
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