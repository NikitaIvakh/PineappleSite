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
                FavouriteHeaderId = 1,
                UserId = "bestuserid1"
            });

            context.FavouriteDetails.AddRange(new FavouriteDetails
            {
                FavouriteDetailsId = 1,
                FavouriteHeaderId = 1,
                ProductId = 1,
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