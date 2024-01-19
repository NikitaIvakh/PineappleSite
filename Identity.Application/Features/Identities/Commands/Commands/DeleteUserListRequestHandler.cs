using AutoMapper;
using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class DeleteUserListRequestHandler(UserManager<ApplicationUser> userManager,
        IDeleteUserListDtoValidator deleteValidator) : IRequestHandler<DeleteUserListRequest, BaseIdentityResponse<UserWithRoles>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IDeleteUserListDtoValidator _deleteValidator = deleteValidator;

        public async Task<BaseIdentityResponse<UserWithRoles>> Handle(DeleteUserListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<UserWithRoles>();

            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteUserList, cancellationToken);

                if (!validator.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка удаления";
                    response.ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList();
                }

                else
                {
                    var users = await _userManager.Users.Where(key => request.DeleteUserList.UserIds.Contains(key.Id)).ToListAsync(cancellationToken) ??
                        throw new Exception("Пользователи не найдены");

                    foreach (var user in users)
                    {
                        var result = await _userManager.DeleteAsync(user);

                        if (!result.Succeeded)
                        {
                            response.IsSuccess = false;
                            response.Message = "Ошибка удаления";
                            response.ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList();

                            return response;
                        }
                    }

                    response.IsSuccess = true;
                    response.Message = "Пользователи успешно удалены";
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