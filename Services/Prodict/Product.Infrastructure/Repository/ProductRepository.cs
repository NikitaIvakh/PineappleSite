using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities.Producrs;
using Product.Domain.Interfaces;

namespace Product.Infrastructure.Repository;

public sealed class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public IQueryable<ProductEntity> GetAll()
    {
        return context.Products.AsNoTracking().AsQueryable();
    }

    public async Task<ProductEntity> CreateAsync(ProductEntity product)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product), @"Объект пустой");
        }

        await context.AddAsync(product);
        await context.SaveChangesAsync();

        return await Task.FromResult(product);
    }

    public async Task<ProductEntity> UpdateAsync(ProductEntity product)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product), @"Объект пустой");
        }

        context.Update(product);
        await context.SaveChangesAsync();

        return await Task.FromResult(product);
    }

    public async Task<ProductEntity> DeleteAsync(ProductEntity product)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product), @"Объект пустой");
        }

        context.Remove(product);
        await context.SaveChangesAsync();

        return await Task.FromResult(product);
    }
}