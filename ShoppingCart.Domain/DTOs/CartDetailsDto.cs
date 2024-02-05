using ShoppingCart.Domain.DTOs;

namespace Favourites.Domain.DTOs
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }

        public CartHeaderDto? CartHeader { get; set; }

        public int CartHeaderId { get; set; }

        public ProductDto? Product { get; set; }

        public int ProductId { get; set; }

        public double Count { get; set; }
    }
}