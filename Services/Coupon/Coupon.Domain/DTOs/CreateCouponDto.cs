namespace Coupon.Domain.DTOs;

public sealed record CreateCouponDto(string CouponCode, double DiscountAmount, double MinAmount);