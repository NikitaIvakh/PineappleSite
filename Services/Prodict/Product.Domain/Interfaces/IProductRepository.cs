using Product.Domain.Entities.Producrs;

namespace Product.Domain.Interfaces;

public interface IProductRepository
{
    IQueryable<ProductEntity> GetAll();

    Task<ProductEntity> CreateAsync(ProductEntity product);

    Task<ProductEntity> UpdateAsync(ProductEntity product);

    Task<ProductEntity> DeleteAsync(ProductEntity product);
}