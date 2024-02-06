namespace PineappleSite.Presentation.Models.ShoppingCart
{
    public class CartViewModel
    {
        public CartHeaderViewModel CartHeader { get; set; }

        public List<CartDetailsViewModel> CartDetails { get; set; }
    }
}