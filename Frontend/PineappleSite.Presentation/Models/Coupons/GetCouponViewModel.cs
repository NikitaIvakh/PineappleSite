namespace PineappleSite.Presentation.Models.Coupons;

public record GetCouponDt(int CouponId, string CouponCode, double DiscountAmount, double MinAmount);