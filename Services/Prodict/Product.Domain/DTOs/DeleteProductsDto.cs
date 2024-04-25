namespace Product.Domain.DTOs;

public sealed record DeleteProductsDto(IList<int> ProductIds);