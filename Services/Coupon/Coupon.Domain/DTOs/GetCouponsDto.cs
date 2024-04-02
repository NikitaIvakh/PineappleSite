namespace Coupon.Domain.DTOs;

public record GetCouponsDto(int CouponId, string CouponCode, double DiscountAmount, double MinAmount);