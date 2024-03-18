using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class RegisterUserRequestHandler(UserManager<ApplicationUser> userManager, IRegisterRequestDtoValidator validationRules, ILogger logger, ApplicationDbContext context) : IRequestHandler<RegisterUserRequest, Result<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IRegisterRequestDtoValidator _registerValidator = validationRules;
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger _logger = logger.ForContext<RegisterUserRequestHandler>();

        public async Task<Result<RegisterResponseDto>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _registerValidator.ValidateAsync(request.RegisterRequest, cancellationToken);

                if (!validator.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>()
                    {
                        {"FirstName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"LastName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"UserName", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"EmailAddress", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"Password", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var message))
                        {
                            return new Result<RegisterResponseDto>
                            {
                                ValidationErrors = message,
                                ErrorMessage = ErrorMessage.RegistrationLoginError,
                                ErrorCode = (int)ErrorCodes.RegistrationLoginError,
                            };
                        }
                    }

                    return new Result<RegisterResponseDto>
                    {
                        ErrorMessage = ErrorMessage.RegistrationLoginError,
                        ErrorCode = (int)ErrorCodes.RegistrationLoginError,
                        ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = request.RegisterRequest.FirstName,
                        LastName = request.RegisterRequest.LastName,
                        UserName = request.RegisterRequest.UserName,
                        Email = request.RegisterRequest.Email,
                        EmailConfirmed = true,
                    };

                    var passwordHasher = new PasswordHasher<ApplicationUser>().HashPassword(user, request.RegisterRequest.Password);
                    user.PasswordHash = passwordHasher;

                    if (request.RegisterRequest.Password == request.RegisterRequest.PasswordConfirm)
                    {
                        var result = await _userManager.CreateAsync(user, request.RegisterRequest.Password);

                        if (!result.Succeeded)
                        {
                            return new Result<RegisterResponseDto>
                            {
                                ErrorCode = (int)ErrorCodes.RegistrationLoginError,
                                ErrorMessage = ErrorMessage.RegistrationLoginError,
                                ValidationErrors = [ErrorMessage.RegistrationLoginError]
                            };
                        }

                        else
                        {
                            var userFromDb = await _userManager.FindByEmailAsync(request.RegisterRequest.Email);

                            if (userFromDb is null)
                            {
                                return new Result<RegisterResponseDto>
                                {
                                    ErrorCode = (int)ErrorCodes.UserNotFound,
                                    ErrorMessage = ErrorMessage.UserNotFound,
                                    ValidationErrors = [ErrorMessage.UserNotFound]
                                };
                            }

                            else
                            {
                                await _userManager.AddToRoleAsync(user, RoleConsts.User);
                                await _context.SaveChangesAsync(cancellationToken);

                                return new Result<RegisterResponseDto>
                                {
                                    Data = new RegisterResponseDto
                                    {
                                        UserId = userFromDb.Id,
                                    },

                                    SuccessMessage = "Вы успешно зарегистрировались",
                                };
                            }
                        }
                    }

                    else
                    {
                        return new Result<RegisterResponseDto>
                        {
                            ErrorCode = (int)ErrorCodes.PasswordsDoNotMatch,
                            ErrorMessage = ErrorMessage.PasswordsDoNotMatch,
                            ValidationErrors = [ErrorMessage.PasswordsDoNotMatch]
                        };
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
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}