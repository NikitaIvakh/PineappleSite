using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Orders
{
    public class BaseOrderService(ILocalStorageService localStorageService, IOrderClient orderClient, IHttpContextAccessor contextAccessor)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IOrderClient _orderClient = orderClient;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        protected OrderResult<OrderHeaderViewModel> ConvertOrderExceptions(OrdersExceptions ordersExceptions)
        {
            if (ordersExceptions.StatusCode == 403)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    ErrorCode = 403,
                    ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                    ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                };
            }

            else if (ordersExceptions.StatusCode == 401)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                };
            }

            else
            {
                return new OrderResult<OrderHeaderViewModel>
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
                _orderClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
            }
        }
    }
}