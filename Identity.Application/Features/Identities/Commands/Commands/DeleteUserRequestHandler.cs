using Identity.Application.DTOs.Identities;
using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class DeleteUserRequestHandler(UserManager<ApplicationUser> userManager, IDeleteUserDtoValidator deleteValidator) : IRequestHandler<DeleteUserRequest, BaseIdentityResponse<DeleteUserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IDeleteUserDtoValidator _deleteValidator = deleteValidator;

        public async Task<BaseIdentityResponse<DeleteUserDto>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<DeleteUserDto>();

            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteUser, cancellationToken);

                if (!validator.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка удаления";
                    response.ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList();
                }

                else
                {
                    var user = await _userManager.FindByIdAsync(request.DeleteUser.Id) ??
                        throw new Exception($"Такого пользователя не существует");

                    var result = await _userManager.DeleteAsync(user);

                    response.IsSuccess = true;
                    response.Message = "Пользователь успешно удален";
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