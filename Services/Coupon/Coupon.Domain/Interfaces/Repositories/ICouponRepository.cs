using Coupon.Domain.Entities;

namespace Coupon.Domain.Interfaces.Repositories
{
    public interface ICouponRepository
    {
        IQueryable<CouponEntity> GetAllAsync();

        Task<CouponEntity> CreateAsync(CouponEntity entity);

        Task<CouponEntity> UpdateAsync(CouponEntity entity);

        Task<CouponEntity> DeleteAsync(CouponEntity entity);
    }
}