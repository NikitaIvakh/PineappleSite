using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.Interface;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LoginUserRequestHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> options, IAuthRequestDtoValidator authValidator, ITokenProvider tokenProvider,
        IHttpContextAccessor httpContextAccessor, ILogger logger) : IRequestHandler<LoginUserRequest, Result<AuthResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JwtSettings _jwtSettings = options.Value;
        private readonly IAuthRequestDtoValidator _authValidator = authValidator;
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger _logger = logger.ForContext<LoginUserRequestHandler>();

        public async Task<Result<AuthResponseDto>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _authValidator.ValidateAsync(request.AuthRequest, cancellationToken);

                if (!validator.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>()
                    {
                        {"Email",  validator.Errors.Select(key => key.ErrorMessage).ToList()},
                        {"Password",  validator.Errors.Select(key => key.ErrorMessage).ToList()},
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var message))
                        {
                            return new Result<AuthResponseDto>
                            {
                                ValidationErrors = message,
                                ErrorMessage = ErrorMessage.AccountLoginError,
                                ErrorCode = (int)ErrorCodes.AccountLoginError,
                            };
                        }
                    }

                    return new Result<AuthResponseDto>
                    {
                        ErrorMessage = ErrorMessage.AccountLoginError,
                        ErrorCode = (int)ErrorCodes.AccountLoginError,
                        ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
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
                        };
                    }

                    else
                    {
                        var result = await _signInManager.CheckPasswordSignInAsync(user, request.AuthRequest.Password, false);

                        if (!result.Succeeded)
                        {
                            return new Result<AuthResponseDto>
                            {
                                ErrorMessage = ErrorMessage.AccountLoginError,
                                ErrorCode = (int)ErrorCodes.AccountLoginError,
                                ValidationErrors = validator.Errors.Select(_ => _.ErrorMessage).ToList(),
                            };
                        }

                        else
                        {
                            JwtSecurityToken jwtSecurityToken = await GenerateTokenAsync(user);
                            AuthResponseDto authResponse = new()
                            {
                                Id = user.Id,
                                Email = user.Email,
                                UserName = user.UserName,
                                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                            };

                            SingInUserAsync(authResponse);
                            _tokenProvider.SetToken(authResponse.Token);

                            return new Result<AuthResponseDto>
                            {
                                Data = authResponse,
                                SuccessMessage = "Уcпешный вход в аккаунт",
                            };
                        }
                    }

                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<AuthResponseDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }

        private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var role = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            for (int i = 0; i < role.Count; i++)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }.Union(userClaims).Union(roleClaims);

            var symmericSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredintials = new SigningCredentials(symmericSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationMinutes),
                signingCredentials: signingCredintials);

            return jwtSecurityToken;
        }

        private async void SingInUserAsync(AuthResponseDto authResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(authResponse.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            var tokenCookieValue = authResponse.Token;
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("TokenCookie", tokenCookieValue, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Измените на true в продакшн среде, если используется HTTPS
                Expires = DateTime.UtcNow.AddDays(1),
                SameSite = SameSiteMode.None,
                Path = "/"
            });

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.First(key => key.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.First(key => key.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.First(key => key.Type == JwtRegisteredClaimNames.Email).Value));

            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}