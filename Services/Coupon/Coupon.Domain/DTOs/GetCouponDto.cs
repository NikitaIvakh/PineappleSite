namespace Coupon.Domain.DTOs
{
    public record GetCouponDto(int CouponId, string CouponCode, double DiscountAmount, double MinAmount);
}