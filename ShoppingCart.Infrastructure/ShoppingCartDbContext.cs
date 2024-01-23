using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Infrastructure
{
    public class ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : DbContext(options), ICartHeaderDbContext, ICartDetailsDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingCartDbContext).Assembly);
        }

        public DbSet<CartHeader> CartHeaders { get; set; }

        public DbSet<CartDetails> CartDetails { get; set; }
    }
}