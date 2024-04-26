using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Domain.Interfaces.Service;

public interface ICouponService
{
    Task<Result<CouponDto>> GetCouponAsync(string? couponCode);
}