namespace Coupon.Domain.DTOs
{
    public interface ICouponDto
    {
        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public double MinAmount { get; set; }
    }
}