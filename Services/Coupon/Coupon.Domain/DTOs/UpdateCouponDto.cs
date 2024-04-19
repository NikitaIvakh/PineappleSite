namespace Coupon.Domain.DTOs;

public sealed record UpdateCouponDto(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);