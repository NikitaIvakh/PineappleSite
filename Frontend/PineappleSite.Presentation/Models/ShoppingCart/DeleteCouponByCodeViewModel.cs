namespace PineappleSite.Presentation.Models.ShoppingCart;

public class DeleteCouponByCodeViewModel(string couponCode)
{
    public string CouponCode { get; set; } = couponCode;
}