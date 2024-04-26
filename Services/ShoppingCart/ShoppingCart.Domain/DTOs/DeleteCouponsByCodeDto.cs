namespace ShoppingCart.Domain.DTOs;

public class DeleteCouponsByCodeDto(List<string> couponCodes)
{
    public List<string> CouponCodes { get; set; } = couponCodes;
}