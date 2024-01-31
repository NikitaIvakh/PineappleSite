using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class RegisterUserRequestHandler(UserManager<ApplicationUser> userManager, IRegisterRequestDtoValidator validationRules, ILogger logger) : IRequestHandler<RegisterUserRequest, Result<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IRegisterRequestDtoValidator _registerValidator = validationRules;
        private readonly ILogger _logger = logger.ForContext<RegisterUserRequestHandler>();

        public async Task<Result<RegisterResponseDto>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _registerValidator.ValidateAsync(request.RegisterRequest, cancellationToken);

                if (!validator.IsValid)
                {
                    return new Result<RegisterResponseDto>
                    {
                        ErrorMessage = ErrorMessage.RegistrationLoginError,
                        ErrorCode = (int)ErrorCodes.RegistrationLoginError,
                        ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var existsUser = await _userManager.FindByNameAsync(request.RegisterRequest.UserName);

                    if (existsUser is not null)
                    {
                        return new Result<RegisterResponseDto>
                        {
                            ErrorMessage = ErrorMessage.UserAlreadyExists,
                            ErrorCode = (int)ErrorCodes.UserAlreadyExists,
                        };
                    }

                    else
                    {
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

                                return new Result<RegisterResponseDto>
                                {
                                    Data = registerResponse,
                                    SuccessMessage = "Вы успешно зарегистрировались",
                                };
                            }

                            else
                            {
                                return new Result<RegisterResponseDto>
                                {
                                    ErrorMessage = ErrorMessage.RegistrationLoginError,
                                    ErrorCode = (int)ErrorCodes.RegistrationLoginError,
                                    ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                                };
                            }
                        }

                        else
                        {
                            return new Result<RegisterResponseDto>
                            {
                                ErrorMessage = ErrorMessage.ThisEmailAddressIsAlreadyExists,
                                ErrorCode = (int)ErrorCodes.ThisEmailAddressIsAlreadyExists,
                            };
                        }
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<RegisterResponseDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}