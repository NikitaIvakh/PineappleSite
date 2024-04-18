using Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coupon.Infrastructure.Configuration;

public class CouponTypeConfiguration : IEntityTypeConfiguration<CouponEntity>
{
    public void Configure(EntityTypeBuilder<CouponEntity> builder)
    {
        builder.ToTable("Coupons");
        builder.HasKey(key => key.CouponId);
        builder.Property(key => key.CouponId).ValueGeneratedOnAdd();
        builder.Property(key => key.CouponCode).HasColumnType("varchar(255)").IsRequired();
    }
}