using Coupon.Domain.Entities;
using Coupon.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Test.Common
{
    public static class CouponRepositoryContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            context.AddRange(new CouponEntity
            {
                CouponId = 3,
                CouponCode = "10OFF",
                DiscountAmount = 10,
                MinAmount = 20,
            },
            new CouponEntity
            {
                CouponId = 4,
                CouponCode = "20OFF",
                DiscountAmount = 20,
                MinAmount = 30,
            },
            new CouponEntity
            {
                CouponId = 5,
                CouponCode = "30OFF",
                DiscountAmount = 30,
                MinAmount = 40,
            });

            context.SaveChanges();
            return context;
        }

        public static void DestroyDatabase(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}