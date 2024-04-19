namespace Coupon.Domain.DTOs;

public record UpdateCouponDto(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);