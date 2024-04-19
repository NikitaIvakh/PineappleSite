namespace Coupon.Domain.DTOs;

public record GetCouponDto(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);