using Favourite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Favourite.Infrastructure.Configuration
{
    public class CartHeaderTypeConfiguration : IEntityTypeConfiguration<FavouriteHeader>
    {
        public void Configure(EntityTypeBuilder<FavouriteHeader> builder)
        {
            builder.HasKey(key => key.FavouriteHeaderId);
            builder.Property(key => key.UserId).IsRequired(false);
        }
    }
}