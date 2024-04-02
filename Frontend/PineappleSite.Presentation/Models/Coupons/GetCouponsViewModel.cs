namespace PineappleSite.Presentation.Models.Coupons;

public record GetCouponsViewModel(int CouponId, string CouponCode, double DiscountAmount, double MinAmount);