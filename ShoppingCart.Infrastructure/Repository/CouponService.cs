using System.Text.Json;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.Infrastructure.Repository
{
    public class CouponService(IHttpClientFactory httpClientFactory) : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<Result<CouponDto>> GetCouponAsync(string couponCode)
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
                return await JsonSerializer.DeserializeAsync<Result<CouponDto>>(stream, options);
            }

            return new Result<CouponDto>();
        }
    }
}