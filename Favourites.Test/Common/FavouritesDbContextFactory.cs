using Favourites.Domain.DTOs;
using Favourites.Domain.Interfaces.Repositories;
using Favourites.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Favourites.Test.Common
{
    public static class FavouritesDbContextFactory
    {
        public static ApplicationDbContext Create(IBaseRepository<FavouritesDto> repository)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            repository.CreateAsync(
                new FavouritesDto()
                {
                    FavoutiteHeader = new FavoutiteHeaderDto
                    {
                        FavouritesHeaderId = 2,
                        UserId = "testuserid2"
                    },

                    FavouritesDetails = new List<FavouritesDetailsDto>
                    {
                        new() {
                            FavouritesDetailsId = 2,
                            FavouritesHeaderId = 2,
                            ProductId = 3,
                        }
                    }
                }).GetAwaiter().GetResult();

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
