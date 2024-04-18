namespace Coupon.Domain.DTOs;

public record CreateCouponDto(string CouponCode, double DiscountAmount, double MinAmount);