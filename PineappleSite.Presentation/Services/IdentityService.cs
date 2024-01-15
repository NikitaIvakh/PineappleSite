using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Services.Identities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace PineappleSite.Presentation.Services
{
    public class IdentityService(ILocalStorageService localStorageService, IIdentityClient identityClient, IMapper mapper,
        IHttpContextAccessor httpContextAccessor) : BaseIdentityService(localStorageService, identityClient), IIdentityService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        public async Task<bool> LoginAsync(AuthRequestViewModel authRequestViewModel)
        {
            try
            {
                AuthRequest authRequest = _mapper.Map<AuthRequest>(authRequestViewModel);
                var authResponse = await _identityClient.LoginAsync(authRequest);

                if (authResponse.Token != string.Empty)
                {
                    var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(authResponse.Token);
                    var claims = ParseClaim(tokenContent);
                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    var login = _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                    _localStorageService.SetStorageValue("token", authResponse.Token);

                    return true;
                }

                return false;
            }

            catch
            {
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterRequestViewModel registerRequestViewModel)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(registerRequestViewModel);
            RegisterResponse registerResponse = await _identityClient.RegisterAsync(registerRequest);

            if (!string.IsNullOrEmpty(registerResponse.UserId))
            {
                var authRequest = new AuthRequestViewModel
                {
                    Email = registerRequestViewModel.EmailAddress,
                    Password = registerRequestViewModel.Password,
                };

                await LoginAsync(authRequest);
                return true;
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            _localStorageService.ClearStorage(["token"]);
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static IEnumerable<Claim> ParseClaim(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));

            return claims;
        }
    }
}