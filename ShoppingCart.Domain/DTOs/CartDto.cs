using ShoppingCart.Domain.Results;

namespace Favourites.Domain.DTOs
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }

        public List<CartDetailsDto> CartDetails { get; set; }
    }
}