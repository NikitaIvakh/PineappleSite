﻿using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Coupons
{
    public class BaseCouponService(ILocalStorageService localStorageService, ICouponClient couponClient, IHttpContextAccessor contextAccessor)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly ICouponClient _couponClient = couponClient;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        protected ResultViewModel<CouponDto> ConvertCouponExceptions(CouponExceptions couponExceptions)
        {
            if (couponExceptions.StatusCode == 403)
            {
                return new ResultViewModel<CouponDto>
                {
                    ErrorCode = 403,
                    ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                    ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                };
            }

            else if (couponExceptions.StatusCode == 401)
            {
                return new ResultViewModel<CouponDto>
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                };
            }

            else
            {
                return new ResultViewModel<CouponDto>()
                {
                    ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз.",
                    ErrorCode = 500
                };
            }
        }

        protected void AddBearerToken()
        {
            if (_contextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
            {
                _couponClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
            }
        }
    }
}