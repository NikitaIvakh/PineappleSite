namespace PineappleSite.Coupon.Core.Entities
{
    public class Coupon
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public double MinAmount { get; set; }
    }
}