namespace PineappleSite.Presentation.Models.Coupons
{
    public class CreateCouponViewModel
    {
        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public double MinAmount { get; set; }
    }
}