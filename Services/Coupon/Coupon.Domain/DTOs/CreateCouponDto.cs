namespace Coupon.Domain.DTOs
{
    public class CreateCouponDto : ICouponDto
    {
        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public double MinAmount { get; set; }
    }
}