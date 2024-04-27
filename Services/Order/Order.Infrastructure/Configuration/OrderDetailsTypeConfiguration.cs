using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Configuration;

public sealed class OrderDetailsTypeConfiguration : IEntityTypeConfiguration<OrderDetails>
{
    public void Configure(EntityTypeBuilder<OrderDetails> builder)
    {
        builder.HasKey(key => key.OrderDetailsId);
        builder.Property(key => key.OrderDetailsId).ValueGeneratedOnAdd();
        builder.HasOne(key => key.OrderHeader).WithMany(key => key.OrderDetails)
            .HasForeignKey(key => key.OrderHeaderId);

        builder.Ignore(key => key.Product);
    }
}