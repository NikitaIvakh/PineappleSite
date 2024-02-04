using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Entities.Cart;
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
            context.CartHeaders.AddRange(
                new CartHeader
                {
                    Id = 1,
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    CouponCode = "10OFF",
                    Discount = 10,
                    CartTotal = 2,
                });

            context.CartDetails.AddRange(
                new CartDetails
                {
                    Id = 1,
                    CartHeaderId = 1,
                    ProductId = 1,
                    Count = 2,
                });

            context.SaveChanges();
            return context;
        }

        public static void Desctroy(ApplicationDbContext shoppingCartDbContext)
        {
            shoppingCartDbContext.Database.EnsureDeleted();
            shoppingCartDbContext.Dispose();
        }
    }
}