using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Contracts;

public interface ICouponService
{
    Task<CollectionResultViewModel<GetCouponsViewModel>> GetAllCouponsAsync();

    Task<ResultViewModel<GetCouponViewModel>> GetCouponAsync(int couponId);

    Task<ResultViewModel<GetCouponViewModel>> GetCouponAsync(string couponCode);

    Task<ResultViewModel<int>> CreateCouponAsync(CreateCouponViewModel createCoupon);

    Task<ResultViewModel> UpdateCouponAsync(int id, UpdateCouponViewModel updateCoupon);

    Task<ResultViewModel> DeleteCouponAsync(int id, DeleteCouponViewModel deleteCoupon);

    Task<CollectionResultViewModel<bool>> DeleteCouponsAsync(DeleteCouponsViewModel deleteCoupons);
}