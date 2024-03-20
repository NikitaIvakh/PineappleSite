using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Products;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Products
{
    public class BaseProductService(ILocalStorageService localStorageService, IProductClient productClient, IHttpContextAccessor contextAccessor)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IProductClient _productClient = productClient;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        protected ProductResultViewModel<ProductViewModel> ConvertProductException(ProductExceptions productExceptions)
        {
            if (productExceptions.StatusCode == 403)
            {
                return new ProductResultViewModel<ProductViewModel>
                {
                    ErrorCode = 403,
                    ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                    ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                };
            }

            else if (productExceptions.StatusCode == 401)
            {
                return new ProductResultViewModel<ProductViewModel>
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                };
            }

            else
            {
                return new ProductResultViewModel<ProductViewModel>
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
                _productClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
            }
        }
    }
}