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

            var orderHeader = new OrderHeader
            {
                OrderHeaderId = 1,
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                CouponCode = "5off",
                Discount = 10,
                OrderTotal = 10,

                Name = "name",
                Email = "email@gmail.com",
                PhoneNumber = "375445679090",
                Address = "test address",
                DeliveryDate = DateTime.UtcNow,

                OrderTime = DateTime.Now,
                Status = StaticDetails.Status_Pending,
                StripeSessionId = "sk_test_51O90F4D1JYWWRL6F1K5vbfmQJeQuN8YRrNQYhq1I3l6OHyRqe6kzhS6wYYelu1YXtjftts7Ela0WDdmIafeGRS6n00AL3kb8tV",
                PaymentIntentId = "pk_test_51O90F4D1JYWWRL6FfzRmE3NEQrVS2SzFgXASN2N3TNgQb4P7UMXXvN7YlVoFKakglHw8UMi7HifHFLN7KnUOznOZ00k3Sexivf",
            };

            var orderDetails = new OrderDetails
            {
                OrderDetailsId = 1,
                OrderHeaderId = 1,
                ProductId = 1,
                ProductName = "name",
                Price = 28,
                Count = 2,
            };

            context.OrderDetails.Add(orderDetails);
            context.OrderHeaders.Add(orderHeader);
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