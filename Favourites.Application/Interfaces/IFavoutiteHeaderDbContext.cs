using Favourites.Domain.Entities.Favourite;
using Microsoft.EntityFrameworkCore;

namespace Favourites.Application.Interfaces
{
    public interface IFavoutiteHeaderDbContext
    {
        DbSet<FavouritesHeader> FavouritesHeaders { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}