using Favourite.Domain.Enum;

namespace Favourite.Domain.DTOs;

public sealed class ProductDto
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public ProductCategory ProductCategory { get; set; }

    public double Price { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageLocalPath { get; set; }
}