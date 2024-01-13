namespace PineappleSite.Presentation.Models.Coupons
{
    public class UpdateCouponViewModel
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public double MinAmount { get; set; }
    }
}