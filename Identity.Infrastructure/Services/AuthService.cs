using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;
using Identity.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<AuthResponse> Login(AuthRequest authRequest)
        {
            var user = await _userManager.FindByEmailAsync(authRequest.Email) ?? throw new Exception($"{nameof(authRequest.Email)} не найден");
            var result = await _signInManager.CheckPasswordSignInAsync(user, authRequest.Password, false);

            if (!result.Succeeded)
            {
                throw new Exception($"{nameof(authRequest.Email)} не существует");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateTokenAsync(user);
            AuthResponse authResponse = new()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };

            return authResponse;
        }

        public async Task<RegisterResponse> Register(RegisterRequest registerRequest)
        {
            var existsUser = await _userManager.FindByNameAsync(registerRequest.UserName);

            if (existsUser is not null)
                throw new Exception($"Такой пользователь уже существует");

            var user = new ApplicationUser
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.UserName,
                Email = registerRequest.EmailAddress,
                EmailConfirmed = true,
            };

            var existsEmail = await _userManager.FindByEmailAsync(registerRequest.EmailAddress);

            if (existsEmail is not null)
            {
                var result = await _userManager.CreateAsync(user, registerRequest.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return new RegisterResponse { UserId = user.Id };
                }

                else
                {
                    throw new Exception($"{result.Errors}");
                }
            }

            else
            {
                throw new Exception($"{registerRequest.EmailAddress} уже используется");
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
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationMinutes),
                signingCredentials: signingCredintials);

            return jwtSecurityToken;
        }
    }
}