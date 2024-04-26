using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<CartHeader> CartHeaders { get; init; }

    public DbSet<CartDetails> CartDetails { get; init; }
}