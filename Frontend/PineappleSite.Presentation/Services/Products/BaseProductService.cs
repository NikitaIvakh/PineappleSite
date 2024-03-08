using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Products;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Products
{
    public class BaseProductService(ILocalStorageService localStorageService, IProductClient productClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IProductClient _productClient = productClient;

        protected ProductResultViewModel<ProductViewModel> ConvertProductException(ProductExceptions productExceptions)
        {
            if (productExceptions.StatusCode == 400)
            {
                return new ProductResultViewModel<ProductViewModel>() { ErrorMessage = "Произошли ошибки валидации.", ValidationErrors = productExceptions.Response };
            }

            else if (productExceptions.StatusCode == 404)
            {
                return new ProductResultViewModel<ProductViewModel>() { ErrorMessage = "Требуемый элемент не удалось найти." };
            }

            else
            {
                return new ProductResultViewModel<ProductViewModel>() { ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз." };
            }
        }

        protected void AddBearerToken()
        {
            if (_localStorageService.Exists("token"))
            {
                _productClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorageService.GetStorageValue<string>("token"));
            }
        }
    }
}