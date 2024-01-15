using Identity.Application.DTOs.Identity;
using Identity.Application.DTOs.Identity.Validator;
using Identity.Application.Features.Identities.Requests.Handlers;
using Identity.Application.Response;
using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Application.Features.Identities.Commands.Handlers
{
    public class LoginCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IAuthRequestDroValidator validator, JwtSettings jwtSettings) : IRequestHandler<LoginCommand, BaseIdentityResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IAuthRequestDroValidator _validator = validator;
        private readonly JwtSettings _jwtSettings = jwtSettings;

        public async Task<BaseIdentityResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse();

            try
            {
                var validationResult = await _validator.ValidateAsync(request.AuthRequest, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка авторизации";
                }

                else
                {
                    var user = await _userManager.FindByEmailAsync(request.AuthRequest.Email) ?? throw new Exception($"{nameof(request.AuthRequest.Email)} не найден");
                    var result = await _signInManager.CheckPasswordSignInAsync(user, request.AuthRequest.Password, false);

                    if (!result.Succeeded)
                    {
                        throw new Exception($"{nameof(AuthRequest.Email)} не существует");
                    }

                    JwtSecurityToken jwtSecurityToken = await GenerateTokenAsync(user);
                    AuthResponse authResponse = new()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    };

                    response.IsSuccess = true;
                    response.Message = "Успешный вход";
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