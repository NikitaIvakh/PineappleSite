using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Test.Common
{
    public static class CouponRepositoryContextFactory
    {
        public static ApplicationDbContext Create(IBaseRepository<CouponEntity> repository)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            repository.CreateAsync(new CouponEntity
            {
                CouponId = 3,
                CouponCode = "10OFF",
                DiscountAmount = 10,
                MinAmount = 20,
            }).GetAwaiter().GetResult();

            repository.CreateAsync(new CouponEntity
            {
                CouponId = 4,
                CouponCode = "20OFF",
                DiscountAmount = 20,
                MinAmount = 30,
            }).GetAwaiter().GetResult();

            repository.CreateAsync(new CouponEntity
            {
                CouponId = 5,
                CouponCode = "30OFF",
                DiscountAmount = 30,
                MinAmount = 40,
            }).GetAwaiter().GetResult();

            return context;
        }

        public static void DestroyDatabase(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}