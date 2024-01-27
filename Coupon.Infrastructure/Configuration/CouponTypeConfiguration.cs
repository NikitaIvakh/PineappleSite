using Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coupon.Infrastructure.Configuration
{
    public class CouponTypeConfiguration : IEntityTypeConfiguration<CouponEntity>
    {
        public void Configure(EntityTypeBuilder<CouponEntity> builder)
        {
            builder.ToTable("Coupons");
            builder.HasKey(key => key.CouponId);
            builder.Property(key => key.CouponCode).HasColumnType("varchar(255)").IsRequired();

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<CouponEntity> builder)
        {
            builder.HasData(new CouponEntity
            {
                CouponId = 1,
                CouponCode = "5OFF",
                DiscountAmount = 2,
                MinAmount = 7,
            },

            new CouponEntity
            {
                CouponId = 2,
                CouponCode = "7OFF",
                DiscountAmount = 5,
                MinAmount = 10,
            });
        }
    }
}