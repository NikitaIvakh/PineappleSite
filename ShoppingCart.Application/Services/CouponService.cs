using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Services.IServices;
using System.Text.Json;

namespace ShoppingCart.Application.Services
{
    public class CouponService(IHttpClientFactory httpClientFactory) : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<CouponDto> GetCouponAsync(string couponCode)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("Coupon");
            var response = await httpClient.GetAsync($"/api/Coupon/GetCouponByCode/{couponCode}");
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                JsonSerializerOptions jsonSerializerOptions = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                var options = jsonSerializerOptions;
                return await JsonSerializer.DeserializeAsync<CouponDto>(stream, options);
            }

            return new CouponDto();
        }
    }
}