using Microsoft.EntityFrameworkCore;
using Product.Core.Entities.Producrs;

namespace Product.Application.Interfaces
{
    public interface IProductDbContext
    {
        DbSet<ProductEntity> Products { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}