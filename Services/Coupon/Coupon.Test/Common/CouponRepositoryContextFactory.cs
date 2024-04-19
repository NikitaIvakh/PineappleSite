using Coupon.Domain.Entities;
using Coupon.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Test.Common;

public static class CouponRepositoryContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        context.AddRange(
            new CouponEntity
            {
                CouponId = Guid.Parse("62646e91-82a6-4fc4-be5a-635241189afe").ToString(),
                CouponCode = "10OFF",
                DiscountAmount = 10,
                MinAmount = 20,
            },

            new CouponEntity
            {
                CouponId = Guid.Parse("284e4b19-fccf-4ac4-8b13-a26dcd9e2475").ToString(),
                CouponCode = "20OFF",
                DiscountAmount = 20,
                MinAmount = 30,
            },

            new CouponEntity
            {
                CouponId = Guid.Parse("a70b2384-54bf-4c01-91be-689ba8dd1a31").ToString(),
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