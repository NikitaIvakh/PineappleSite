using Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Domain.Interfaces.Database
{
    public interface ICouponDbContext
    {
        DbSet<CouponEntity> Coupons { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}