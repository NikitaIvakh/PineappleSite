using Favourite.Domain.Entities;
using Favourite.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Favourite.Test.Common
{
    public static class FavouriteProductsDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            context.FavouriteHeaders.AddRange(new FavouriteHeader
            {
                FavouriteHeaderId = 3,
                UserId = "bestuserid1"
            });

            context.FavouriteDetails.AddRange(new FavouriteDetails
            {
                FavouriteDetailsId = 3,
                FavouriteHeaderId = 3,
                ProductId = 4,
            });

            context.SaveChanges();
            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}