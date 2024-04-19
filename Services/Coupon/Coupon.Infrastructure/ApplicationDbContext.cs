using Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Infrastructure;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<CouponEntity> Coupons { get; init; }
}