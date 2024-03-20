using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.ShoppingCart;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public class BaseShoppingCartService(ILocalStorageService localStorageService, IShoppingCartClient shoppingCartClient, IHttpContextAccessor contextAccessor)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IShoppingCartClient _shoppingCartClient = shoppingCartClient;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public CartResult<CartViewModel> ConvertShoppingCartExceptions(ShoppingCartExceptions exceptions)
        {
            if (exceptions.StatusCode == 403)
            {
                return new CartResult<CartViewModel>
                {
                    ErrorCode = 403,
                    ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                    ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                };
            }

            else if (exceptions.StatusCode == 401)
            {
                return new CartResult<CartViewModel>
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                };
            }

            else
            {
                return new CartResult<CartViewModel>
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
                _shoppingCartClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
            }
        }
    }
}