using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Configuration;

public sealed class CartHeaderTypeConfiguration : IEntityTypeConfiguration<CartHeader>
{
    public void Configure(EntityTypeBuilder<CartHeader> builder)
    {
        builder.HasKey(key => key.CartHeaderId);
        builder.Property(key => key.UserId).IsRequired(false);
        builder.Property(key => key.CouponCode).IsRequired(false);
        builder.Property(key => key.Discount).HasColumnType("numeric(10, 2)");
        builder.Property(key => key.CartTotal).HasColumnType("numeric(10, 2)");

        builder.Ignore(key => key.Discount);
        builder.Ignore(key => key.CartTotal);
    }
}