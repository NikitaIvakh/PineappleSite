using Favourites.Domain.Entities.Favourite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Favourites.Infrastructure.Configuration
{
    public class FavouritesDetailsEntityTypeConfiguration : IEntityTypeConfiguration<FavouritesDetails>
    {
        public void Configure(EntityTypeBuilder<FavouritesDetails> builder)
        {
            builder.HasKey(key => key.FavouritesDetailsId);
            builder.HasOne(key => key.FavouritesHeader).WithMany().HasForeignKey(key => key.FavouritesHeaderId);

            builder.Ignore(key => key.Product);
        }
    }
}