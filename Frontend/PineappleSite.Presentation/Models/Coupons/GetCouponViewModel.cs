namespace PineappleSite.Presentation.Models.Coupons;

public record GetCouponViewModel(int CouponId, string CouponCode, double DiscountAmount, double MinAmount);