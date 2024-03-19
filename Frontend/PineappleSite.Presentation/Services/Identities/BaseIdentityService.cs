using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Identities
{
    public class BaseIdentityService(ILocalStorageService localStorageService, IIdentityClient identityClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;

        protected IdentityResult ConvertIdentityExceptions(IdentityExceptions identityExceptions)
        {
            if (identityExceptions.StatusCode == 400)
            {
                return new IdentityResult() { ErrorMessage = "Произошли ошибки валидации.", ValidationErrors = [identityExceptions.Response] };
            }

            else if (identityExceptions.StatusCode == 404)
            {
                return new IdentityResult() { ErrorMessage = "Требуемый элемент не удалось найти." };
            }

            else
            {
                return new IdentityResult() { ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз." };
            }
        }

        protected void AddBearerToken()
        {
            if (_localStorageService.Exists("token"))
            {
                _identityClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorageService.GetStorageValue<string>("token"));
            }
        }
    }
}