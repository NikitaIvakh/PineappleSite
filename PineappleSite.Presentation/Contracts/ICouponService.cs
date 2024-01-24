using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Contracts
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponViewModel>> GetAllCouponsAsync();

        Task<CouponViewModel> GetCouponAsync(int couponId);

        Task<CouponViewModel> GetCouponAsync(string couponCode);

        Task<ResponseViewModel> CreateCouponAsync(CreateCouponViewModel createCoupon);

        Task<ResponseViewModel> UpdateCouponAsync(int id, UpdateCouponViewModel updateCoupon);

        Task<ResponseViewModel> DeleteCouponAsync(int id, DeleteCouponViewModel deleteCoupon);

        Task<ResponseViewModel> DeleteCouponsAsync(DeleteCouponListViewModel deleteCouponList);
    }
}