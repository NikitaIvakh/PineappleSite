using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models.ShoppingCart;

public sealed class CartDetailsViewModel
{
    
    public int CartDetailsId { get; set; }
    

    public CartViewModel? CartHeader { get; init; }
    

    public int CartHeaderId { get; set; }

    public ProductViewModel? Product { get; set; }

    public int ProductId { get; init; }

    public double Count { get; set; }
}