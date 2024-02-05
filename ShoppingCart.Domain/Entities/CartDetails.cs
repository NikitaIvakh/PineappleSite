using ShoppingCart.Domain.DTOs;

namespace ShoppingCart.Domain.Entities
{
    public class CartDetails
    {
        public int CartDetailsId { get; set; }

        public CartHeader? CartHeader { get; set; }

        public int CartHeaderId { get; set; }

        public ProductDto? Product { get; set; }

        public int ProductId { get; set; }
    }
}