using Identity.Application.Extecsions;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Services;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LoginUserRequestHandler(UserManager<ApplicationUser> userManager, IAuthRequestDtoValidator authValidator, ApplicationDbContext context, ITokenService tokenService, IConfiguration configuration) : IRequestHandler<LoginUserRequest, Result<AuthResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IAuthRequestDtoValidator _authValidator = authValidator;

        public async Task<Result<AuthResponseDto>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _authValidator.ValidateAsync(request.AuthRequest, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var existErrorMessages = new Dictionary<string, List<string>>
                    {
                        {"Email", validationResult.Errors.Select(key => key.ErrorMessage).ToList() },
                        {"Password", validationResult.Errors.Select(key => key.ErrorMessage).ToList() },
                    };

                    foreach (var error in existErrorMessages)
                    {
                        if (existErrorMessages.TryGetValue(error.Key, out var message))
                        {
                            return new Result<AuthResponseDto>
                            {
                                Data = null,
                                ValidationErrors = message,
                                ErrorMessage = ErrorMessage.AccountLoginError,
                                ErrorCode = (int)ErrorCodes.AccountLoginError,
                            };
                        }
                    }

                    return new Result<AuthResponseDto>
                    {
                        Data = null,
                        ErrorMessage = ErrorMessage.AccountLoginError,
                        ErrorCode = (int)ErrorCodes.AccountLoginError,
                        ValidationErrors = [ErrorMessage.AccountLoginError],
                    };
                }

                else
                {
                    var user = await _userManager.FindByEmailAsync(request.AuthRequest.Email);

                    if (user is null)
                    {
                        return new Result<AuthResponseDto>
                        {
                            ErrorMessage = ErrorMessage.UserNotFound,
                            ErrorCode = (int)ErrorCodes.UserNotFound,
                            ValidationErrors = [ErrorMessage.UserNotFound]
                        };
                    }

                    else
                    {
                        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.AuthRequest.Password.Trim());

                        if (!isValidPassword)
                        {
                            return new Result<AuthResponseDto>
                            {
                                ErrorCode = (int)ErrorCodes.AccountLoginError,
                                ErrorMessage = ErrorMessage.AccountLoginError,
                                ValidationErrors = [ErrorMessage.AccountLoginError]
                            };
                        }

                        else
                        {
                            var roleIds = await _context.UserRoles.Where(key => key.UserId == user.Id).Select(key => key.RoleId).ToListAsync(cancellationToken);
                            var roles = await _context.Roles.Where(key => roleIds.Contains(key.Id)).ToListAsync(cancellationToken);

                            var accessToken = _tokenService.CreateToken(user, roles);
                            user.RefreshToken = _configuration.GenerateRefreshToken();
                            user.RefreshTokenExpiresTime = DateTime.UtcNow.AddDays(int.Parse(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Value!));

                            await _context.SaveChangesAsync(cancellationToken);

                            return new Result<AuthResponseDto>
                            {
                                Data = new AuthResponseDto
                                {
                                    FirstName = user.FirstName.Trim(),
                                    LastName = user.LastName.Trim(),
                                    UsertName = user.UserName!.Trim(),
                                    Email = user.Email!.Trim(),
                                    JwtToken = accessToken,
                                    RefreshToken = user.RefreshToken,
                                },

                                SuccessMessage = "Вы успешно вошли в аккаунт"
                            };
                        }
                    }
                }
            }

            catch
            {
                return new Result<AuthResponseDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}