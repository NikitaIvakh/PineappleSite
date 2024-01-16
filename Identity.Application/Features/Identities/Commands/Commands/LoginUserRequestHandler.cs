using AutoMapper;
using Identity.Application.DTOs.Authentications;
using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LoginUserRequestHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> options,
        IAuthRequestDtoValidator authValidator) : IRequestHandler<LoginUserRequest, BaseIdentityResponse<AuthResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JwtSettings _jwtSettings = options.Value;
        private readonly IAuthRequestDtoValidator _authValidator = authValidator;

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
                        throw new Exception($"{nameof(request.AuthRequest.Email)} не существует");
                    }

                    JwtSecurityToken jwtSecurityToken = await GenerateTokenAsync(user);
                    AuthResponseDto authResponse = new()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    };

                    response.IsSuccess = true;
                    response.Message = "Уcпешный вход";
                    response.Data = authResponse;

                    return response;
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
    }
}