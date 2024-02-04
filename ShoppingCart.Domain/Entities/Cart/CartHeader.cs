namespace ShoppingCart.Domain.Entities.Cart
{
    public class CartHeader
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string? CouponCode { get; set; }

        public double Discount { get; set; }

        public double CartTotal { get; set; }
    }
}