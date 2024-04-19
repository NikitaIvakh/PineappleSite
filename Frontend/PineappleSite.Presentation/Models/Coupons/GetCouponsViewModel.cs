namespace PineappleSite.Presentation.Models.Coupons;

public record GetCouponsViewModel(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);