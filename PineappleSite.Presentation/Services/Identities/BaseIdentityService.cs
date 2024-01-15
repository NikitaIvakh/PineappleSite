using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Identities
{
    public class BaseIdentityService(ILocalStorageService localStorageService, IIdentityClient identityClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;

        protected IdentityResponseViewModel ConvertIdentityExceptions(IdentityExceptions identityExceptions)
        {
            if (identityExceptions.StatusCode == 400)
            {
                return new IdentityResponseViewModel() { Message = "Произошли ошибки валидации.", ValidationErrors = identityExceptions.Response, IsSuccess = false };
            }

            else if (identityExceptions.StatusCode == 404)
            {
                return new IdentityResponseViewModel() { Message = "Требуемый элемент не удалось найти.", IsSuccess = false };
            }

            else
            {
                return new IdentityResponseViewModel() { Message = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", IsSuccess = false };
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