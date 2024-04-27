namespace Order.Domain.DTOs;

public sealed class CartDetailsDto
{
    public int CartDetailsId { get; init; }
    

    public CartHeaderDto? CartHeader { get; init; }

    public int CartHeaderId { get; init; }

    public ProductDto? Product { get; init; }

    public int ProductId { get; init; }

    public double Count { get; init; }
}