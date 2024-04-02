namespace Coupon.Domain.DTOs
{
    public record UpdateCouponDto(int CouponId, string CouponCode, double DiscountAmount, double MinAmount);
}