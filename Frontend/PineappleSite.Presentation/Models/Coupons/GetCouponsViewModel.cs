namespace PineappleSite.Presentation.Models.Coupons;

public sealed record GetCouponsViewModel(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);