using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Products
{
    public class BaseProductService(ILocalStorageService localStorageService, IProductClient productClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IProductClient _productClient = productClient;

        protected ProductAPIViewModel ConvertProductException(ProductExceptions productExceptions)
        {
            if (productExceptions.StatusCode == 400)
            {
                return new ProductAPIViewModel() { Message = "Произошли ошибки валидации.", ValidationErrors = productExceptions.Response, IsSuccess = false };
            }

            else if (productExceptions.StatusCode == 404)
            {
                return new ProductAPIViewModel() { Message = "Требуемый элемент не удалось найти.", IsSuccess = false };
            }

            else
            {
                return new ProductAPIViewModel() { Message = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", IsSuccess = false };
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