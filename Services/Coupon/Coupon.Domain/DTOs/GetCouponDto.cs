namespace Coupon.Domain.DTOs;

public sealed record GetCouponDto(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);