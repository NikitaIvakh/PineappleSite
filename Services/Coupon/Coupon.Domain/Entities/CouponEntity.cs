namespace Coupon.Domain.Entities;

public class CouponEntity
{
    public int CouponId { get; init; }

    public required string CouponCode { get; set; }

    public required double DiscountAmount { get; set; }

    public required double MinAmount { get; set; }
}