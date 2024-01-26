using Favourites.Domain.Entities.Favourite;
using Microsoft.EntityFrameworkCore;

namespace Favourites.Application.Interfaces
{
    public interface IFavoutiteDetailsDbContext
    {
        DbSet<FavouritesDetails> FavouritesDetails { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}