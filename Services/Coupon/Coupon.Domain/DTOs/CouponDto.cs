namespace Coupon.Domain.DTOs;

public sealed class CouponDto
{
    public required string CouponId { get; init; }

    public required string CouponCode { get; init; }

    public required double DiscountAmount { get; init; }

    public required double MinAmount { get; init; }
}