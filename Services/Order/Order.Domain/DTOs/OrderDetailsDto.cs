namespace Order.Domain.DTOs;

public sealed class OrderDetailsDto
{
    public int OrderDetailsId { get; init; }
    

    public int OrderHeaderId { get; init; }

    public ProductDto? Product { get; init; }
    

    public int ProductId { get; init; }

    public int Count { get; init; }

    public string? ProductName { get; init; }

    public double Price { get; init; }
}