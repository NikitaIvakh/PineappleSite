using Product.Domain.Enum;

namespace Product.Domain.DTOs;

public interface IProductDto
{
    public string Name { get; }

    public string Description { get; }

    public ProductCategory ProductCategory { get; }

    public double Price { get; }
}