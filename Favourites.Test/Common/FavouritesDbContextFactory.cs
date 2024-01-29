using Favourites.Domain.Entities.Favourite;
using Favourites.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Favourites.Test.Common
{
    public static class FavouritesDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            context.AddRange(new FavouritesHeader
            {
                FavouritesHeaderId = 3,
                UserId = "testuserid",
            });

            context.AddRange(new FavouritesDetails
            {
                FavouritesDetailsId = 3,
                FavouritesHeaderId = 3,
                ProductId = 1,
            });

            context.SaveChanges();
            return context;
        }

        public static void DestroyDatabase(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}