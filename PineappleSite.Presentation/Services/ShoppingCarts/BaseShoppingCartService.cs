using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.ShoppingCart;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public class BaseShoppingCartService(ILocalStorageService localStorageService, IShoppingCartClient shoppingCartClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IShoppingCartClient _shoppingCartClient = shoppingCartClient;

        protected CartResultViewModel<CartViewModel> ConvertShoppingCartException(ShoppingCartExceptions shoppingCartExceptions)
        {
            if (shoppingCartExceptions.StatusCode == 400)
            {
                return new CartResultViewModel<CartViewModel>() { ErrorMessage = "Произошли ошибки валидации.", ValidationErrors = shoppingCartExceptions.Response };
            }

            else if (shoppingCartExceptions.StatusCode == 404)
            {
                return new CartResultViewModel<CartViewModel>() { ErrorMessage = "Требуемый элемент не удалось найти." };
            }

            else
            {
                return new CartResultViewModel<CartViewModel>() { ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз." };
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