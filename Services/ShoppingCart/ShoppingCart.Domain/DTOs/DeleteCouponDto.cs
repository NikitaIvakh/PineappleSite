namespace ShoppingCart.Domain.DTOs;

public class DeleteCouponDto(string couponCode)
{
    public string CouponCode { get; set; } = couponCode;
}