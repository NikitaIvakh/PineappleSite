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

        public async Task<IdentityResult<AuthResponseViewModel>> LoginAsync(AuthRequestViewModel authRequestViewModel)
        {
            try
            {
                AddBearerToken();
                AuthRequestDto authRequest = _mapper.Map<AuthRequestDto>(authRequestViewModel);
                AuthResponseDtoResult authResponse = await _identityClient.LoginAsync(authRequest);

                if (authResponse.IsSuccess)
                {
                    if (authResponse.Data.Token != string.Empty)
                    {
                        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(authResponse.Data.Token);
                        var claims = ParseClaim(tokenContent);
                        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                        var login = _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                        _localStorageService.SetStorageValue("token", authResponse.Data.Token);

                        return new IdentityResult<AuthResponseViewModel>
                        {
                            SuccessMessage = authResponse.SuccessMessage,
                            Data = _mapper.Map<AuthResponseViewModel>(authResponse),
                        };
                    }

                    else
                    {
                        foreach (string error in authResponse.ValidationErrors)
                        {
                            return new IdentityResult<AuthResponseViewModel>
                            {
                                ErrorCode = authResponse.ErrorCode,
                                ErrorMessage = authResponse.ErrorMessage,
                                ValidationErrors = error + Environment.NewLine,
                            };
                        }
                    }
                }

                else
                {
                    foreach (string error in authResponse.ValidationErrors)
                    {
                        return new IdentityResult<AuthResponseViewModel>
                        {
                            ErrorCode = authResponse.ErrorCode,
                            ErrorMessage = authResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<AuthResponseViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityResult<AuthResponseViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<IdentityResult<RegisterResponseViewModel>> RegisterAsync(RegisterRequestViewModel registerRequestViewModel)
        {
            try
            {
                AddBearerToken();
                RegisterRequestDto registerRequest = _mapper.Map<RegisterRequestDto>(registerRequestViewModel);
                RegisterResponseDtoResult registerResponse = await _identityClient.RegisterAsync(registerRequest);

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

                        return new IdentityResult<RegisterResponseViewModel>
                        {
                            SuccessMessage = registerResponse.SuccessMessage,
                            Data = _mapper.Map<RegisterResponseViewModel>(registerResponse),
                        };
                    }

                    else
                    {
                        foreach (var errors in registerResponse.ValidationErrors)
                        {
                            return new IdentityResult<RegisterResponseViewModel>
                            {
                                ErrorCode = registerResponse.ErrorCode,
                                ErrorMessage = registerResponse.ErrorMessage,
                                ValidationErrors = errors + Environment.NewLine,
                            };
                        }
                    }
                }

                else
                {
                    foreach (string error in registerResponse.ValidationErrors)
                    {
                        return new IdentityResult<RegisterResponseViewModel>
                        {
                            ErrorCode = registerResponse.ErrorCode,
                            ErrorMessage = registerResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<RegisterResponseViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityResult<RegisterResponseViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<IdentityResult<bool>> LogoutAsync()
        {
            try
            {
                AddBearerToken();
                BooleanResult logoutResult = await _identityClient.LogoutAsync();

                if (logoutResult.IsSuccess)
                {
                    _localStorageService.ClearStorage(["token"]);
                    await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    return new IdentityResult<bool>
                    {
                        Data = true,
                        SuccessMessage = logoutResult.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in logoutResult.ValidationErrors)
                    {
                        return new IdentityResult<bool>
                        {
                            ErrorCode = logoutResult.ErrorCode,
                            ErrorMessage = logoutResult.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<bool>();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityResult<bool>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        private static IEnumerable<Claim> ParseClaim(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));

            return claims;
        }
    }
}