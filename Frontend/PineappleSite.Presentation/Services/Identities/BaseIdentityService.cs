using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Identities;

public class BaseIdentityService(
    ILocalStorageService localStorageService,
    IIdentityClient identityClient,
    IHttpContextAccessor contextAccessor)
{
    protected static IdentityResult ConvertIdentityExceptions(IdentityExceptions<string> identityExceptions)
    {
        return identityExceptions.StatusCode switch
        {
            403 => new IdentityResult
            {
                StatusCode = 403,
                ErrorMessage =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам.",
                ValidationErrors =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам."
            },

            401 => new IdentityResult
            {
                StatusCode = 401,
                ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                ValidationErrors = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."
            },

            _ => new IdentityResult
            {
                StatusCode = 500,
                ErrorMessage = identityExceptions.Result,
                ValidationErrors = identityExceptions.Result,
            }
        };
    }

    protected void AddBearerToken()
    {
        if (contextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
        {
            identityClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
        }
    }
}