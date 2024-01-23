using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Application.Interfaces
{
    public interface ICartDetailsDbContext
    {
        DbSet<CartDetails> CartDetails { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}