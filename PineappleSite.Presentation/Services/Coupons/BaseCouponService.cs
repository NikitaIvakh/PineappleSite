using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Coupons
{
    public class BaseCouponService(ILocalStorageService localStorageService, ICouponClient couponClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly ICouponClient _couponClient = couponClient;

        protected ResultViewModel<CouponDto> ConvertCouponExceptions(CouponExceptions couponExceptions)
        {
            if (couponExceptions.StatusCode == 400)
            {
                return new ResultViewModel<CouponDto>() { ErrorMessage = "Произошли ошибки валидации.", ErrorCode = 400, ValidationErrors = couponExceptions.Response,  };
            }

            else if (couponExceptions.StatusCode == 404)
            {
                return new ResultViewModel<CouponDto>() { ErrorMessage = "Требуемый элемент не удалось найти.", ErrorCode = 404 };
            }

            else
            {
                return new ResultViewModel<CouponDto>() { ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", ErrorCode = 500 };
            }
        }

        protected void AddBearerToken()
        {
            if (_localStorageService.Exists("token"))
            {
                _couponClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorageService.GetStorageValue<string>("token"));
            }
        }
    }
}