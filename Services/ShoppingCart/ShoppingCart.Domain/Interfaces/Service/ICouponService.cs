using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Domain.Interfaces.Service;

public interface ICouponService
{
    Task<Result<CouponDto>> GetCouponByCode(string? couponCode);
}