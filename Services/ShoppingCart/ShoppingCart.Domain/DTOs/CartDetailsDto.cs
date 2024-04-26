namespace ShoppingCart.Domain.DTOs;

public sealed class CartDetailsDto
{
    public int CartDetailsId { get; set; }

    public CartHeaderDto? CartHeader { get; init; }

    public int CartHeaderId { get; set; }

    public ProductDto? Product { get; set; }

    public int ProductId { get; init; }

    public double Count { get; set; }
}