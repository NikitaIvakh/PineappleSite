using Microsoft.EntityFrameworkCore;
using Order.Application.Utility;
using Order.Domain.Entities;
using Order.Infrastructure;

namespace Orders.Test.Common;

public static class OrdersDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        context.AddRange(new OrderHeader
            {
                OrderHeaderId = 1,
                UserId = "testuser5t654",
                CouponCode = "5off",
                Discount = 10,
                OrderTotal = 14,

                Name = "name",
                Email = "email@gmail.com",
                PhoneNumber = "375445679090",
                Address = "test address",
                DeliveryDate = DateTime.UtcNow,

                OrderTime = DateTime.Now,
                Status = StaticDetails.StatusPending,
                StripeSessionId = "test",
                PaymentIntentId = "test",
            },
            
            new OrderDetails
            {
                OrderDetailsId = 1,
                OrderHeaderId = 1,
                ProductId = 2,
                ProductName = "name",
                Price = 28,
                Count = 2,
            });

        context.SaveChanges();
        return context;
    }

    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}