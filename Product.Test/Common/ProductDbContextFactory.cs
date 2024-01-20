using Microsoft.EntityFrameworkCore;
using Product.Core.Entities.Enum;
using Product.Core.Entities.Producrs;
using Product.Infrastructure;

namespace Product.Test.Common
{
    public static class ProductDbContextFactory
    {
        public static PineAppleProductsDbContext Create()
        {
            var options = new DbContextOptionsBuilder<PineAppleProductsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new PineAppleProductsDbContext(options);
            context.Database.EnsureCreated();
            context.AddRange(
                new ProductEntity
                {
                    Id = 4,
                    Name = "Test 1",
                    Description = "Test product 1",
                    ProductCategory = ProductCategory.Soups,
                    Price = 10,
                },

                new ProductEntity
                {
                    Id = 5,
                    Name = "Test 2",
                    Description = "Test product 2",
                    ProductCategory = ProductCategory.Snacks,
                    Price = 15,
                },

                new ProductEntity
                {
                    Id = 6,
                    Name = "Test 3",
                    Description = "Test product 3",
                    ProductCategory = ProductCategory.Drinks,
                    Price = 9,
                });

            context.SaveChanges();
            return context;
        }

        public static void Destroy(PineAppleProductsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}