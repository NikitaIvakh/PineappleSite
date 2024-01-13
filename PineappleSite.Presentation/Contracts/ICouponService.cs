using PineappleSite.Presentation.Models;
using PineappleSite.Presentation.Models.Coupons;

namespace PineappleSite.Presentation.Contracts
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponViewModel>> GetAllCouponsAsync();

        Task<CouponViewModel> GetCouponAsync(int couponId);

        Task<ResponseViewModel> CreateCouponAsync(CreateCouponViewModel createCoupon);

        Task<ResponseViewModel> UpdateCouponAsync(int id, UpdateCouponViewModel updateCoupon);

        Task<ResponseViewModel> DeleteCouponAsync(int id, DeleteCouponViewModel deleteCoupon);
    }
}