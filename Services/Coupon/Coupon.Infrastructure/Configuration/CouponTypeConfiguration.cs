using Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coupon.Infrastructure.Configuration;

public sealed class CouponTypeConfiguration : IEntityTypeConfiguration<CouponEntity>
{
    public void Configure(EntityTypeBuilder<CouponEntity> builder)
    {
        builder.ToTable("Coupons");
        builder.HasKey(key => key.CouponId);
        builder.Property(key => key.DiscountAmount).HasColumnType("numeric(10, 2)");
        builder.Property(key => key.MinAmount).HasColumnType("numeric(10, 2)");
    }
}