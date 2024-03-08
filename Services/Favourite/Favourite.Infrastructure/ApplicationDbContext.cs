using Favourite.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Favourite.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<FavouriteHeader> FavouriteHeaders { get; set; }

        public DbSet<FavouriteDetails> FavouriteDetails { get; set; }
    }
}