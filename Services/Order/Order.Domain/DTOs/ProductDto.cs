using Order.Domain.Enum;

namespace Order.Domain.DTOs;

public sealed class ProductDto
{
    public int Id { get; set; }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public ProductCategory ProductCategory { get; set; }

    public double Price { get; init; }

    public string? ImageUrl { get; set; }

    public string? ImageLocalPath { get; set; }
}