using Identity.Application.DTOs.Authentications;
using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class RegisterUserRequestHandler(UserManager<ApplicationUser> userManager, IRegisterRequestDtoValidator validationRules) : IRequestHandler<RegisterUserRequest, BaseIdentityResponse<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IRegisterRequestDtoValidator _registerValidator = validationRules;

        public async Task<BaseIdentityResponse<RegisterResponseDto>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<RegisterResponseDto>();

            try
            {
                var validator = await _registerValidator.ValidateAsync(request.RegisterRequest, cancellationToken);

                if (!validator.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка входа в аккаунт";
                    response.ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList();
                }

                else
                {
                    var existsUser = await _userManager.FindByNameAsync(request.RegisterRequest.UserName);

                    if (existsUser is not null)
                        throw new Exception($"Такой пользователь уже существует");

                    var user = new ApplicationUser
                    {
                        FirstName = request.RegisterRequest.FirstName,
                        LastName = request.RegisterRequest.LastName,
                        UserName = request.RegisterRequest.UserName,
                        Email = request.RegisterRequest.EmailAddress,
                        EmailConfirmed = true,
                    };

                    var existsEmail = await _userManager.FindByEmailAsync(request.RegisterRequest.EmailAddress);

                    if (existsEmail is null)
                    {
                        var result = await _userManager.CreateAsync(user, request.RegisterRequest.Password);

                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Employee");
                            RegisterResponseDto registerResponse = new()
                            {
                                UserId = user.Id
                            };

                            response.IsSuccess = true;
                            response.Message = "Успешная регистрация";
                            response.Data = registerResponse;

                            return response;
                        }

                        else
                            throw new Exception($"{result.Errors}");
                    }

                    else
                        throw new Exception($"{request.RegisterRequest.EmailAddress} уже используется");
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