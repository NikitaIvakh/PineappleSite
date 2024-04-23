using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Services.Identities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace PineappleSite.Presentation.Services;

public sealed class IdentityService(
    ILocalStorageService localStorageService,
    IIdentityClient identityClient,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor)
    : BaseIdentityService(localStorageService, identityClient, httpContextAccessor), IIdentityService
{
    private readonly IIdentityClient _identityClient = identityClient;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public async Task<IdentityResult<string>> LoginAsync(AuthRequestViewModel authRequestViewModel)
    {
        try
        {
            var authRequest = mapper.Map<AuthRequestDto>(authRequestViewModel);
            var authResponse = await _identityClient.LoginAsync(authRequest);

            if (!authResponse.IsSuccess)
            {
                return new IdentityResult<string>
                {
                    StatusCode = authResponse.StatusCode,
                    ErrorMessage = authResponse.ErrorMessage,
                    ValidationErrors = string.Join(", ", authResponse.ValidationErrors),
                };
            }

            if (string.IsNullOrWhiteSpace(authResponse.Data))
            {
                return new IdentityResult<string>
                {
                    StatusCode = authResponse.StatusCode,
                    ErrorMessage = authResponse.ErrorMessage,
                    ValidationErrors = string.Join(", ", authResponse.ValidationErrors),
                };
            }

            try
            {
                var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(authResponse.Data);
                var user = new ClaimsPrincipal(new ClaimsIdentity(tokenContent.Claims, CookieAuthenticationDefaults.AuthenticationScheme));
                await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddHours(1),
                    Secure = true,
                    HttpOnly = true,
                };

                _httpContextAccessor.HttpContext!.Response.Cookies.Append("JWTToken", tokenContent.RawData,
                    cookieOptions);
            }

            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Ошибка: {ex.Message}");
            }

            return new IdentityResult<string>
            {
                Data = authResponse.Data,
                StatusCode = authResponse.StatusCode,
                SuccessMessage = authResponse.SuccessMessage,
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult<string>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result
            };
        }
    }

    public async Task<IdentityResult<string>> RegisterAsync(RegisterRequestViewModel registerRequestViewModel)
    {
        try
        {
            var registerRequest = mapper.Map<RegisterRequestDto>(registerRequestViewModel);
            var registerResponse = await _identityClient.RegisterAsync(registerRequest);

            if (!registerResponse.IsSuccess)
            {
                return new IdentityResult<string>
                {
                    StatusCode = registerResponse.StatusCode,
                    ErrorMessage = registerResponse.ErrorMessage,
                    ValidationErrors = string.Join(", ", registerResponse.ValidationErrors),
                };
            }

            if (!string.IsNullOrEmpty(registerResponse.Data))
            {
                var authRequest = new AuthRequestViewModel
                {
                    EmailAddress = registerRequestViewModel.EmailAddress,
                    Password = registerRequestViewModel.Password,
                };

                await LoginAsync(authRequest);
            }

            else
            {
                return new IdentityResult<string>
                {
                    StatusCode = registerResponse.StatusCode,
                    ErrorMessage = registerResponse.ErrorMessage,
                    ValidationErrors = string.Join(", ", registerResponse.ValidationErrors),
                };
            }

            return new IdentityResult<string>
            {
                StatusCode = registerResponse.StatusCode,
                ErrorMessage = registerResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", registerResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult<string>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<IdentityResult<ObjectResult>> RefreshTokenAsync(TokenModelViewModel tokenModelViewModel)
    {
        try
        {
            var tokenModelDto = mapper.Map<TokenModelDto>(tokenModelViewModel);
            var apiResponse = await _identityClient.RefreshTokenAsync(tokenModelDto);

            if (apiResponse.IsSuccess)
            {
                return new IdentityResult<ObjectResult>
                {
                    Data = apiResponse.Data,
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityResult<ObjectResult>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult<ObjectResult>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result
            };
        }
    }

    public async Task<IdentityResult> RevokeTokenAsync(string userName)
    {
        try
        {
            var apiResponse = await _identityClient.RevokeTokenAsync(userName);

            if (apiResponse.IsSuccess)
            {
                return new IdentityResult
                {
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<IdentityResult> RevokeAllTokensAsync()
    {
        try
        {
            var apiResponse = await _identityClient.RevokeTokensAsync();

            if (apiResponse.IsSuccess)
            {
                return new IdentityResult
                {
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }
}