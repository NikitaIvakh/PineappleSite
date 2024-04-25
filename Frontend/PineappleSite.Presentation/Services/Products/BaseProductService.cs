using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Products;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Products;

public class BaseProductService(
    ILocalStorageService localStorageService,
    IProductClient productClient,
    IHttpContextAccessor contextAccessor)
{
    protected static ProductResultViewModel<ProductViewModel> ConvertProductException(
        ProductExceptions<string> productExceptions)
    {
        return productExceptions.StatusCode switch
        {
            403 => new ProductResultViewModel<ProductViewModel>
            {
                StatusCode = 403,
                ErrorMessage =
                    "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                ValidationErrors =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам."
            },

            401 => new ProductResultViewModel<ProductViewModel>
            {
                StatusCode = 401,
                ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                ValidationErrors = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."
            },

            _ => new ProductResultViewModel<ProductViewModel>
            {
                StatusCode = 500,
                ErrorMessage = productExceptions.Result,
                ValidationErrors = productExceptions.Result
            }
        };
    }

    protected void AddBearerToken()
    {
        if (contextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
        {
            productClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
        }
    }
}