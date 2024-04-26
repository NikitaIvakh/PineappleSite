namespace PineappleSite.Presentation.Models.ShoppingCart;

public sealed class CartViewModel
{
    public required CartHeaderViewModel CartHeader { get; init; } 

    public required List<CartDetailsViewModel> CartDetails { get; set; }
}