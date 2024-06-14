using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Configuration;

public sealed class OrderHeaderTypeConfiguration : IEntityTypeConfiguration<OrderHeader>
{
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.HasKey(key => key.OrderHeaderId);
        builder.Property(key => key.UserId).IsRequired(false);
        builder.Property(key => key.CouponCode).IsRequired(false);

        builder.Property(key => key.Discount).HasColumnType("numeric(10, 2)");
        builder.Property(key => key.OrderTotal).HasColumnType("numeric(10, 2)");
    }
}