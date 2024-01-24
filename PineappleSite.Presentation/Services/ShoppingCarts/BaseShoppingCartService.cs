using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public class BaseShoppingCartService(ILocalStorageService localStorageService, IShoppingCartClient shoppingCartClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IShoppingCartClient _shoppingCartClient = shoppingCartClient;

        protected ShoppingCartResponseViewModel ConvertShoppingCartException(ShoppingCartExceptions shoppingCartExceptions)
        {
            if (shoppingCartExceptions.StatusCode == 400)
            {
                return new ShoppingCartResponseViewModel() { Message = "Произошли ошибки валидации.", ValidationErrors = shoppingCartExceptions.Response, IsSuccess = false };
            }

            else if (shoppingCartExceptions.StatusCode == 404)
            {
                return new ShoppingCartResponseViewModel() { Message = "Требуемый элемент не удалось найти.", IsSuccess = false };
            }

            else
            {
                return new ShoppingCartResponseViewModel() { Message = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", IsSuccess = false };
            }
        }

        protected void AddBearerToken()
        {
            if (_localStorageService.Exists("token"))
            {
                _shoppingCartClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorageService.GetStorageValue<string>("token"));
            }
        }
    }
}