namespace PineappleSite.Presentation.Models.Coupons;

public record GetCouponViewModel(string CouponId, string CouponCode, double DiscountAmount, double MinAmount);