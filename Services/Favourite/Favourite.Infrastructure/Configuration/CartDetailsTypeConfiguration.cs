using Favourite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Favourite.Infrastructure.Configuration
{
    public class CartDetailsTypeConfiguration : IEntityTypeConfiguration<FavouriteDetails>
    {
        public void Configure(EntityTypeBuilder<FavouriteDetails> builder)
        {
            builder.HasKey(key => key.FavouriteDetailsId);
            builder.HasOne(key => key.FavouriteHeader).WithMany().HasForeignKey(key => key.FavouriteHeaderId);

            builder.Ignore(key => key.Product);
        }
    }
}