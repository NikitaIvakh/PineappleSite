using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Domain.Entities.Cart;

namespace ShoppingCart.Infrastructure.Configuration
{
    public class CratDetailsDbContextConfiguration : IEntityTypeConfiguration<CartDetails>
    {
        public void Configure(EntityTypeBuilder<CartDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Ignore(key => key.Product);
            builder.HasOne(cd => cd.CartHeader).WithMany().HasForeignKey(cd => cd.CartHeaderId);
        }
    }
}