using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Identities
{
    public class BaseIdentityService(ILocalStorageService localStorageService, IIdentityClient identityClient, IHttpContextAccessor contextAccessor)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        protected IdentityResult ConvertIdentityExceptions(IdentityExceptions identityExceptions)
        {
            if (identityExceptions.StatusCode == 403)
            {
                return new IdentityResult
                {
                    ErrorCode = 403,
                    ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                    ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                };
            }

            else if (identityExceptions.StatusCode == 401)
            {
                return new IdentityResult
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                };
            }

            else
            {
                return new IdentityResult
                {
                    ErrorCode = 500,
                    ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз.",
                    ValidationErrors = ["Что-то пошло не так, пожалуйста, попробуйте еще раз."]
                };
            }
        }

        protected void AddBearerToken()
        {
            if (_contextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
            {
                _identityClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
            }
        }
    }
}