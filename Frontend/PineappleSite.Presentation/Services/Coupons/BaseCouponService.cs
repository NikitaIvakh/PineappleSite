using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Coupons;

public class BaseCouponService(
    ILocalStorageService localStorageService,
    ICouponClient couponClient,
    IHttpContextAccessor contextAccessor)
{
    protected static ResultViewModel ConvertCouponExceptions(CouponExceptions<string> couponExceptions)
    {
        return couponExceptions.StatusCode switch
        {
            403 => new ResultViewModel
            {
                StatusCode = 403,
                ErrorMessage =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам.",
                ValidationErrors =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам."
            },

            401 => new ResultViewModel
            {
                StatusCode = 401,
                ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                ValidationErrors = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."
            },

            _ => new ResultViewModel
            {
                StatusCode = 500,
                ErrorMessage = couponExceptions.Result,
                ValidationErrors = couponExceptions.Result,
            }
        };
    }

    protected void AddBearerToken()
    {
        if (contextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
        {
            couponClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
        }
    }
}