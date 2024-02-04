using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.Domain.DTOs
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }

        public CollectionResult<CartDetailsDto> CartDetails { get; set; }
    }
}