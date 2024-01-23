using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Infrastructure.Configuration
{
    public class CratDetailsDbContextConfiguration : IEntityTypeConfiguration<CartDetails>
    {
        public void Configure(EntityTypeBuilder<CartDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Ignore(key => key.Product);
        }
    }
}