namespace PineappleSite.Presentation.Models.ShoppingCart
{
    public class CartViewModel
    {
        public CartHeaderViewModel CartHeader { get; set; }

        public IEnumerable<CartDetailsViewModel> CartDetails { get; set; }
    }
}