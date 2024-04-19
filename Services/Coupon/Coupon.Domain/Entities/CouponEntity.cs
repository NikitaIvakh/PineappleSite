namespace Coupon.Domain.Entities;

public class CouponEntity
{
    public string CouponId { get; init; } = Guid.NewGuid().ToString();

    public required string CouponCode { get; set; }

    public required double DiscountAmount { get; set; }

    public required double MinAmount { get; set; }
}