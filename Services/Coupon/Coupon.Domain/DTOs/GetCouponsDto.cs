namespace Coupon.Domain.DTOs;

public record GetCouponsDto(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);