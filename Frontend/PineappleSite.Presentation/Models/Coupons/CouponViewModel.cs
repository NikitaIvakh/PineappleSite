namespace PineappleSite.Presentation.Models.Coupons;

public sealed class CouponViewModel
{
    public required string CouponId { get; init; }

    public required string CouponCode { get; init; }

    public required double DiscountAmount { get; init; }

    public required double MinAmount { get; init; }
}