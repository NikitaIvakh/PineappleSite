using ShoppingCart.Domain.Enum;

namespace ShoppingCart.Domain.DTOs;

public sealed class ProductDto
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }
    

    public ProductCategory ProductCategory { get; init; }

    public double Price { get; init; }
    

    public string? ImageUrl { get; init; }
    

    public string? ImageLocalPath { get; init; }
}