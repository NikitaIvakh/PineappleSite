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

        public async Task<IdentityResponseViewModel> LoginAsync(AuthRequestViewModel authRequestViewModel)
        {
            AddBearerToken();
            IdentityResponseViewModel response = new();
            AuthRequestDto authRequest = _mapper.Map<AuthRequestDto>(authRequestViewModel);
            AuthResponseDtoBaseIdentityResponse authResponse = await _identityClient.LoginAsync(authRequest);

            if (authResponse.IsSuccess)
            {
                if (authResponse.Data.Token != string.Empty)
                {
                    var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(authResponse.Data.Token);
                    var claims = ParseClaim(tokenContent);
                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    var login = _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                    _localStorageService.SetStorageValue("token", authResponse.Data.Token);

                    response.IsSuccess = true;
                    response.Data = authResponse;
                    response.Message = authResponse.Message;

                    return response;
                }
            }

            else
            {
                foreach (string error in authResponse.ValidationErrors)
                {
                    response.ValidationErrors += error + Environment.NewLine;
                }

                response.Message = authResponse.Message;
            }

            return response;
        }

        public async Task<IdentityResponseViewModel> RegisterAsync(RegisterRequestViewModel registerRequestViewModel)
        {
            AddBearerToken();
            IdentityResponseViewModel response = new();
            RegisterRequestDto registerRequest = _mapper.Map<RegisterRequestDto>(registerRequestViewModel);
            RegisterResponseDtoBaseIdentityResponse registerResponse = await _identityClient.RegisterAsync(registerRequest);

            if (registerResponse.IsSuccess)
            {
                if (!string.IsNullOrEmpty(registerResponse.Data.UserId))
                {
                    var authRequest = new AuthRequestViewModel
                    {
                        Email = registerRequestViewModel.EmailAddress,
                        Password = registerRequestViewModel.Password,
                    };

                    await LoginAsync(authRequest);

                    response.IsSuccess = true;
                    response.Data = authRequest;
                    response.Message = registerResponse.Message;

                    return response;
                }
            }

            else
            {
                foreach (string erro in registerResponse.ValidationErrors)
                {
                    response.ValidationErrors += erro + Environment.NewLine;
                }
            }

            response.IsSuccess = false;
            return response;
        }

        public async Task LogoutAsync()
        {
            AddBearerToken();
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