using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Coupons
{
    public class BaseCouponService(ILocalStorageService localStorageService, ICouponClient couponClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly ICouponClient _couponClient = couponClient;

        protected ResponseViewModel ConvertApiExceptions<Guid>(CouponExceptions couponExceptions)
        {
            if (couponExceptions.StatusCode == 400)
            {
                return new ResponseViewModel() { Message = "Произошли ошибки валидации.", ValidationErrors = couponExceptions.Response, IsSuccess = false };
            }

            else if (couponExceptions.StatusCode == 404)
            {
                return new ResponseViewModel() { Message = "Требуемый элемент не удалось найти.", IsSuccess = false };
            }

            else
            {
                return new ResponseViewModel() { Message = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", IsSuccess = false };
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