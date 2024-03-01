using Microsoft.EntityFrameworkCore;
using Order.Application.Utility;
using Order.Domain.Entities;
using Order.Infrastructure;

namespace Orders.Test.Common
{
    public static class OrdersDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            context.AddRange(
                new OrderHeader
                {
                    OrderHeaderId = 1,
                    UserId = "30B522ED-D37D-4A6D-82D6-75F0FBEF23A6",
                    CouponCode = "5off",
                    Discount = 10,
                    OrderTotal = 10,

                    Name = "name",
                    Email = "email",
                    PhoneNumber = "375445679090",

                    OrderTime = DateTime.Now,
                    Status = StaticDetails.Status_Pending,
                    StripeSessionId = "sk_test_51O90F4D1JYWWRL6F1K5vbfmQJeQuN8YRrNQYhq1I3l6OHyRqe6kzhS6wYYelu1YXtjftts7Ela0WDdmIafeGRS6n00AL3kb8tV",
                    PaymentIntentId = "pk_test_51O90F4D1JYWWRL6FfzRmE3NEQrVS2SzFgXASN2N3TNgQb4P7UMXXvN7YlVoFKakglHw8UMi7HifHFLN7KnUOznOZ00k3Sexivf",
                    OrderDetails =
                    [
                        new() {
                            OrderDetailsId = 1,
                            OrderHeaderId = 1,
                            ProductId = 1,
                            ProductName = "name",
                            Price = 28,
                            Count = 2,
                        },
                    ],
                });

            context.AddRange(
                new OrderDetails
                {
                    OrderDetailsId = 1,
                    OrderHeaderId = 1,
                    ProductId = 1,
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
}