namespace PineappleSite.Presentation.Models.Coupons;

public sealed record GetCouponViewModel(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);