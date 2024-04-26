using ShoppingCart.Domain.DTOs;

namespace ShoppingCart.Domain.Entities;

public class CartDetails
{
    public int CartDetailsId { get; init; }

    public CartHeader? CartHeader { get; init; }

    public int CartHeaderId { get; init; }

    public ProductDto? Product { get; init; }

    public int ProductId { get; init; }

    public double Count { get; init; }
}