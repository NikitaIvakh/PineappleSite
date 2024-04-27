using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Orders;

public class BaseOrderService(
    ILocalStorageService localStorageService,
    IOrderClient orderClient,
    IHttpContextAccessor contextAccessor)
{
    protected OrderResult ConvertOrderExceptions(OrdersExceptions<string> ordersExceptions)
    {
        return ordersExceptions.StatusCode switch
        {
            403 => new OrderResult<OrderHeaderViewModel>
            {
                StatusCode = 403,
                ErrorMessage =
                    "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                ValidationErrors =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам."
            },

            401 => new OrderResult<OrderHeaderViewModel>
            {
                StatusCode = 401,
                ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                ValidationErrors = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."
            },

            _ => new OrderResult<OrderHeaderViewModel>
            {
                StatusCode = 500,
                ErrorMessage = ordersExceptions.Result,
                ValidationErrors = ordersExceptions.Result
            }
        };
    }

    protected void AddBearerToken()
    {
        if (contextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
        {
            orderClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
        }
    }
}