using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure;

namespace ShoppingCart.Test.Common
{
    public static class ShoppingCartDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            context.CartHeaders.AddRange
                (
                    new CartHeader
                    {
                        CartHeaderId = 1,
                        UserId = "TestuserId23",
                        CouponCode = "5OFF",
                        Discount = 10,
                        CartTotal = 20
                    }
                );

            context.CartDetails.AddRange
                (
                    new CartDetails
                    {
                        CartDetailsId = 1,
                        CartHeaderId = 1,
                        ProductId = 3,
                        Count = 1,
                    }
                );

            context.SaveChanges();
            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}