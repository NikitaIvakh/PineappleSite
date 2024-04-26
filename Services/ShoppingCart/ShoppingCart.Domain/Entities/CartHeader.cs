namespace ShoppingCart.Domain.Entities;

public class CartHeader
{
    public int CartHeaderId { get; init; }

    public string? UserId { get; init; }

    public string? CouponCode { get; set; }

    public double Discount { get; init; }

    public double CartTotal { get; init; }
}