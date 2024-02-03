using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Contracts
{
    public interface ICouponService
    {
        Task<CollectionResultViewModel<CouponViewModel>> GetAllCouponsAsync();

        Task<ResultViewModel<CouponViewModel>> GetCouponAsync(int couponId);

        Task<ResultViewModel<CouponViewModel>> GetCouponAsync(string couponCode);

        Task<ResultViewModel<CouponViewModel>> CreateCouponAsync(CreateCouponViewModel createCoupon);

        Task<ResultViewModel<CouponViewModel>> UpdateCouponAsync(int id, UpdateCouponViewModel updateCoupon);

        Task<ResultViewModel<CouponViewModel>> DeleteCouponAsync(int id, DeleteCouponViewModel deleteCoupon);

        Task<CollectionResultViewModel<CouponViewModel>> DeleteCouponsAsync(DeleteCouponListViewModel deleteCouponList);
    }
}