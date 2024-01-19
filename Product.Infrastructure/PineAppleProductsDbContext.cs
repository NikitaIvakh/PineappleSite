using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces;
using Product.Core.Entities.Producrs;

namespace Product.Infrastructure
{
    public class PineAppleProductsDbContext(DbContextOptions<PineAppleProductsDbContext> options) : DbContext(options), IProductDbContext
    {
        public DbSet<ProductEntity> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PineAppleProductsDbContext).Assembly);
        }
    }
}