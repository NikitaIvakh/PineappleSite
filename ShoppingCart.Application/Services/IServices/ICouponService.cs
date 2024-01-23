using ShoppingCart.Application.DTOs.Cart;

namespace ShoppingCart.Application.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponAsync(string couponCode);
    }
}