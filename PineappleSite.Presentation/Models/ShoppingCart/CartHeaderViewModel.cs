namespace PineappleSite.Presentation.Models.ShoppingCart
{
    public class CartHeaderViewModel
    {
        public int CartHeaderId { get; set; }

        public string? UserId { get; set; }

        public string? CouponCode { get; set; }

        public double Discount { get; set; }

        public double CartTotal { get; set; }
    }
}