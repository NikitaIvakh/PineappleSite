using ShoppingCart.Domain.DTOs;

namespace ShoppingCart.Domain.Entities.Cart
{
    public class CartDetails
    {
        public int Id { get; set; }

        public CartHeader CartHeader { get; set; }

        public int CartHeaderId { get; set; }

        public ProductDto Product { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }
    }
}