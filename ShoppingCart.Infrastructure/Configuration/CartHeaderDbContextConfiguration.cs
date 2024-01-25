using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Infrastructure.Configuration
{
    public class CartHeaderDbContextConfiguration : IEntityTypeConfiguration<CartHeader>
    {
        public void Configure(EntityTypeBuilder<CartHeader> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(key => key.UserId).IsRequired(false);
            builder.Property(key => key.CouponCode).IsRequired(false);

            builder.Ignore(key => key.Discount);
            builder.Ignore(key => key.CartTotal);
        }
    }
}