using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Orders
{
    public class BaseOrderService(ILocalStorageService localStorageService, IOrderClient orderClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IOrderClient _orderClient = orderClient;

        protected OrderResult<OrderHeaderViewModel> ConvertOrderExceptions(OrdersExceptions ordersExceptions)
        {
            if (ordersExceptions.StatusCode == 400)
            {
                return new OrderResult<OrderHeaderViewModel>() { ErrorMessage = "Произошли ошибки валидации.", ErrorCode = 400, ValidationErrors = ordersExceptions.Response, };
            }

            else if (ordersExceptions.StatusCode == 404)
            {
                return new OrderResult<OrderHeaderViewModel>() { ErrorMessage = "Требуемый элемент не удалось найти.", ErrorCode = 404 };
            }

            else
            {
                return new OrderResult<OrderHeaderViewModel>() { ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", ErrorCode = 500 };
            }
        }

        protected void AddBearerToken()
        {
            if (_localStorageService.Exists("token"))
            {
                _orderClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorageService.GetStorageValue<string>("token"));
            }
        }
    }
}