using Microsoft.AspNetCore.Http;
using Product.Domain.Enum;

namespace Product.Domain.DTOs;

public sealed record CreateProductDto(
    string Name,
    string Description,
    ProductCategory ProductCategory,
    double Price,
    IFormFile? Avatar) : IProductDto;