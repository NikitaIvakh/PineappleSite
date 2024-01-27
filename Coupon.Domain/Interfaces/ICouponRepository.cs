using Coupon.Domain.Entities;

namespace PineappleSite.Coupon.Core.Interfaces
{
    public interface ICouponRepository
    {
        Task<IEnumerable<CouponEntity>> GetAllAsync();

        Task<CouponEntity> GetAsync(int couponId);

        Task CreateAsync(CouponEntity coupon);

        Task UpdateAsync(CouponEntity coupon);

        Task DeleteAsync(int couponId);
    }
}