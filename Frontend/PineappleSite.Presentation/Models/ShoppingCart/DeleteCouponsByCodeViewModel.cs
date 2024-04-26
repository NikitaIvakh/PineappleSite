namespace PineappleSite.Presentation.Models.ShoppingCart;

public sealed class DeleteCouponsByCodeViewModel(List<string> couponCodes)
{
    public List<string> CouponCodes { get; set; } = couponCodes;
}