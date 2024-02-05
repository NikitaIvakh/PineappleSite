using ShoppingCart.Domain.Results;

namespace Favourites.Domain.DTOs
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }

        public CollectionResult<CartDetailsDto> CartDetails { get; set; }
    }
}