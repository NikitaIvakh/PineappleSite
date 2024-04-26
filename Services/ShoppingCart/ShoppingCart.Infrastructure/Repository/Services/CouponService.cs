using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Domain.Results;
using System.Text.Json;

namespace ShoppingCart.Infrastructure.Repository.Services;

public sealed class CouponService(IHttpClientFactory httpClientFactory) : ICouponService
{
    public async Task<Result<CouponDto>> GetCouponByCode(string? couponCode)
    {
        var httpClient = httpClientFactory.CreateClient("Coupon");
        var response = await httpClient.GetAsync($"/api/coupons/GetCouponByCode/{couponCode}");

        if (!response.IsSuccessStatusCode)
        {
            return new Result<CouponDto>();
        }

        await using var stream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        return (await JsonSerializer.DeserializeAsync<Result<CouponDto>>(stream, jsonSerializerOptions))!;
    }
}