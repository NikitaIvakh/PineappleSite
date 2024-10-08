﻿using Microsoft.AspNetCore.Http;
using Product.Domain.Enum;

namespace Product.Domain.DTOs;

public sealed record UpdateProductDto(
    int Id,
    string Name,
    string Description,
    ProductCategory ProductCategory,
    double Price,
    IFormFile? Avatar,
    string? ImageUrl,
    string? ImageLocalPath) : IProductDto;