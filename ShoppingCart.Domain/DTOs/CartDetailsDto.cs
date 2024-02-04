namespace ShoppingCart.Domain.DTOs
{
    public class CartDetailsDto
    {
        public int Id { get; set; }

        public CartHeaderDto? CartHeader { get; set; }

        public int CartHeaderId { get; set; }

        public ProductDto? Product { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }
    }
}