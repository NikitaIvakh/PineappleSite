namespace Coupon.Domain.DTOs
{
    public class UpdateCouponDto : ICouponDto
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public double MinAmount { get; set; }
    }
}