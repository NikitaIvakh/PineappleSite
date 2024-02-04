using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICouponService
    {
        Task<Result<CouponDto>> GetCouponAsync(string couponCode);
    }
}