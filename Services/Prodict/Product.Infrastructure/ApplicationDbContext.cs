using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities.Producrs;

namespace Product.Infrastructure;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<ProductEntity> Products { get; init; }
}