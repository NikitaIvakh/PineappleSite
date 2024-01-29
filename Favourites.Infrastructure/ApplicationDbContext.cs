using Favourites.Domain.Entities.Favourite;
using Microsoft.EntityFrameworkCore;

namespace Favourites.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<FavouritesHeader> FavouritesHeaders { get; set; }

        public DbSet<FavouritesDetails> FavouritesDetails { get; set; }
    }
}