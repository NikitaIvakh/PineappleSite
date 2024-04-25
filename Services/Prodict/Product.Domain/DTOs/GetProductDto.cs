using Product.Domain.Enum;

namespace Product.Domain.DTOs;

public sealed record GetProductDto(
    int Id,
    string Name,
    string Description,
    ProductCategory ProductCategory,
    double Price,
    string? ImageUrl,
    string? ImageLocalPath) : IProductDto;