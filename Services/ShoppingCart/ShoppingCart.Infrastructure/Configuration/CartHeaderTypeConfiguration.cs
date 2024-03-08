using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Configuration
{
    public class CartHeaderTypeConfiguration : IEntityTypeConfiguration<CartHeader>
    {
        public void Configure(EntityTypeBuilder<CartHeader> builder)
        {
            builder.HasKey(key => key.CartHeaderId);
            builder.Property(key => key.UserId).IsRequired(false);
            builder.Property(key => key.CouponCode).IsRequired(false);

            builder.Ignore(key => key.Discount);
            builder.Ignore(key => key.CartTotal);
        }
    }
}