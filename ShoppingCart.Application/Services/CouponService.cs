using Newtonsoft.Json;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Response;
using ShoppingCart.Application.Services.IServices;

namespace ShoppingCart.Application.Services
{
    public class CouponService(IHttpClientFactory httpClientFactory) : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<CouponDto> GetCouponAsync(string couponCode)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("Coupon");
            var response = await httpClient.GetAsync($"api/Coupon/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ShoppingCartAPIResponse>(apiContent);

            if (apiResponse is not null && apiResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(apiResponse.Data));
            }

            return new CouponDto();
        }
    }
}