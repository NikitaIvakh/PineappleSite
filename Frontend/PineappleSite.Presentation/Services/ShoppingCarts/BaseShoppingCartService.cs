using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.ShoppingCart;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public class BaseShoppingCartService(ILocalStorageService localStorageService, IShoppingCartClient shoppingCartClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IShoppingCartClient _shoppingCartClient = shoppingCartClient;

        public CartResult<CartViewModel> ConvertShoppingCartExceptions(ShoppingCartExceptions exceptions)
        {
            if (exceptions.StatusCode == 400)
            {
                return new CartResult<CartViewModel>() { ErrorMessage = "Произошли ошибки валидации.", ErrorCode = 400, ValidationErrors = [exceptions.Response], };
            }

            else if (exceptions.StatusCode == 404)
            {
                return new CartResult<CartViewModel>() { ErrorMessage = "Требуемый элемент не удалось найти.", ErrorCode = 404 };
            }

            else
            {
                return new CartResult<CartViewModel>() { ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", ErrorCode = 500 };
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