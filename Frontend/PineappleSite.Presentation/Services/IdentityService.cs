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
    public class IdentityService(ILocalStorageService localStorageService, IIdentityClient identityClient, IMapper mapper, IHttpContextAccessor httpContextAccessor) : BaseIdentityService(localStorageService, identityClient), IIdentityService
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
                    if (!string.IsNullOrWhiteSpace(authResponse.Data.JwtToken))
                    {
                        try
                        {
                            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(authResponse.Data.JwtToken);
                            var user = new ClaimsPrincipal(new ClaimsIdentity(tokenContent.Claims, CookieAuthenticationDefaults.AuthenticationScheme));
                            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

                            var cookieOptions = new CookieOptions
                            {
                                Expires = DateTime.UtcNow.AddHours(1),
                                Secure = true,
                                HttpOnly = true,
                            };

                            _httpContextAccessor.HttpContext!.Response.Cookies.Append("JWTToken", tokenContent.RawData, cookieOptions);
                        }

                        catch (Exception ex)
                        {
                            await Console.Out.WriteLineAsync($"Ошибка: {ex.Message}");
                        }

                        return new IdentityResult<AuthResponseViewModel>
                        {
                            SuccessMessage = authResponse.SuccessMessage,
                            Data = _mapper.Map<AuthResponseViewModel>(authResponse.Data),
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

                return new IdentityResult<RegisterResponseViewModel>()
                {
                    SuccessMessage = registerResponse.SuccessMessage,
                };
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

        public async Task<IdentityResult<ObjectResult>> RefreshTokenAsync(TokenModelViewModel tokenModelViewModel)
        {
            try
            {
                TokenModelDto tokenModelDto = _mapper.Map<TokenModelDto>(tokenModelViewModel);
                ObjectResultResult apiResponse = await _identityClient.RefreshTokenAsync(tokenModelDto);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult<ObjectResult>
                    {
                        Data = apiResponse.Data,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<ObjectResult>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<ObjectResult>();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityResult<ObjectResult>
                {
                    ErrorCode = exceptions.StatusCode,
                    ErrorMessage = exceptions.Response,
                    ValidationErrors = exceptions.Response
                };
            }
        }

        public async Task<IdentityResult> RevokeTokenAsync(string userName)
        {
            try
            {
                UnitResult apiResponse = await _identityClient.RevokeTokenAsync(userName);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityResult
                {
                    ErrorCode = exceptions.StatusCode,
                    ErrorMessage = exceptions.Response,
                    ValidationErrors = exceptions.Response,
                };
            }
        }

        public async Task<IdentityResult> RevokeAllTokensAsync()
        {
            try
            {
                var apiResponse = await _identityClient.RevokeAllTokensAsync();

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityResult
                {
                    ErrorCode = exceptions.StatusCode,
                    ErrorMessage = exceptions.Response,
                    ValidationErrors = exceptions.Response,
                };
            }
        }
    }
}