﻿using Identity.Application.DTOs.Authentications;
using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Application.Services.IServices;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LoginUserRequestHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> options,
        IAuthRequestDtoValidator authValidator, ITokenProvider tokenProvider,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<LoginUserRequest, BaseIdentityResponse<AuthResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JwtSettings _jwtSettings = options.Value;
        private readonly IAuthRequestDtoValidator _authValidator = authValidator;
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<BaseIdentityResponse<AuthResponseDto>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<AuthResponseDto>();

            try
            {
                var validator = await _authValidator.ValidateAsync(request.AuthRequest, cancellationToken);

                if (!validator.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка входа в аккаунт";
                    response.ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList();
                }

                else
                {
                    var user = await _userManager.FindByEmailAsync(request.AuthRequest.Email) ?? throw new Exception($"Пользователь с почтой ({nameof(request.AuthRequest.Email)}) не найден");
                    var result = await _signInManager.CheckPasswordSignInAsync(user, request.AuthRequest.Password, false);

                    if (!result.Succeeded)
                    {
                        response.IsSuccess = false;
                        response.Message = "Неверно введен логин или пароль";
                        response.Data = new AuthResponseDto();
                        response.ValidationErrors = validator.Errors.Select(_ => _.ErrorMessage).ToList();
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

                        response.IsSuccess = true;
                        response.Message = "Уcпешный вход";
                        response.Data = authResponse;

                        return response;
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
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationMinutes),
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