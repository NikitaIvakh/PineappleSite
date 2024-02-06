using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models.ShoppingCart
{
    public class CartDetailsViewModel
    {
        public int CartDetailsId { get; set; }

        public CartHeaderViewModel? CartHeader { get; set; }

        public int CartHeaderId { get; set; }

        public ProductViewModel? Product { get; set; }

        public int ProductId { get; set; }

        public double Count { get; set; }
    }
}