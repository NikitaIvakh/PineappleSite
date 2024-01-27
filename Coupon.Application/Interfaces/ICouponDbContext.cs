using Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Interfaces
{
    public interface ICouponDbContext
    {
        DbSet<CouponEntity> Coupons { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}