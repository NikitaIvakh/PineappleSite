using Coupon.Application.Interfaces;
using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), ICouponDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<CouponEntity> Coupons { get; set; }
    }
}