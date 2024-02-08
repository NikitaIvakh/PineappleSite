using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Configuration
{
    public class OrderDetailsTypeConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.HasKey(key => key.OrderDetailsId);
            builder.HasOne(key => key.OrderHeader).WithMany().HasForeignKey(key => key.OrderHeaderId);

            builder.Ignore(key => key.Product);
        }
    }
}