using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Application.Interfaces
{
    public interface ICartHeaderDbContext
    {
        public DbSet<CartHeader> CartHeaders { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}