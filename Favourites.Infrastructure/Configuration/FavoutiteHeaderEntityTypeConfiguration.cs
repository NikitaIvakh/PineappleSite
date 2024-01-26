using Favourites.Domain.Entities.Favourite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Favourites.Infrastructure.Configuration
{
    public class FavoutiteHeaderEntityTypeConfiguration : IEntityTypeConfiguration<FavouritesHeader>
    {
        public void Configure(EntityTypeBuilder<FavouritesHeader> builder)
        {
            builder.HasKey(key => key.FavouritesHeaderId);
            builder.Property(key => key.UserId).IsRequired(false);
        }
    }
}