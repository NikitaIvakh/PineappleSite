using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.ShoppingCart;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.ShoppingCarts;

public class BaseShoppingCartService(
    ILocalStorageService localStorageService,
    IShoppingCartClient shoppingCartClient,
    IHttpContextAccessor contextAccessor)
{
    protected static CartResult<CartViewModel> ConvertShoppingCartExceptions(ShoppingCartExceptions<string> exceptions)
    {
        return exceptions.StatusCode switch
        {
            403 => new CartResult<CartViewModel>
            {
                StatusCode = 403,
                ErrorMessage =
                    "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                ValidationErrors =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам."
            },

            401 => new CartResult<CartViewModel>
            {
                StatusCode = 401,
                ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                ValidationErrors = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."
            },

            _ => new CartResult<CartViewModel>
            {
                StatusCode = 500,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            }
        };
    }

    protected void AddBearerToken()
    {
        if (contextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
        {
            shoppingCartClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
        }
    }
}