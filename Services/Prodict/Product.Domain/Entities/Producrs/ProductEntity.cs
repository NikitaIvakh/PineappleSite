using Product.Domain.Enum;

namespace Product.Domain.Entities.Producrs;

public sealed class ProductEntity
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public ProductCategory ProductCategory { get; set; }

    public double Price { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageLocalPath { get; set; }
}