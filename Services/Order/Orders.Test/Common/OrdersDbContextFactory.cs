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
                StripeSessionId = "test",
                PaymentIntentId = "test",
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