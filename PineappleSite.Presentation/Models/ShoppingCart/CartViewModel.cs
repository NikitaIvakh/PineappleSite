namespace PineappleSite.Presentation.Models.ShoppingCart
{
    public class CartViewModel
    {
        public CartHeaderViewModel CartHeader { get; set; }

        public IReadOnlyCollection<CartDetailsViewModel>? CartDetails { get; set; }
    }
}