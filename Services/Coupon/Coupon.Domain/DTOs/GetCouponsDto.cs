namespace Coupon.Domain.DTOs;

public sealed record GetCouponsDto(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);