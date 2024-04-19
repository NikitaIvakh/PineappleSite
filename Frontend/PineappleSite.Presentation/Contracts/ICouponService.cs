using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Contracts;

public interface ICouponService
{
    Task<CollectionResultViewModel<GetCouponsViewModel>> GetAllCouponsAsync();

    Task<ResultViewModel<GetCouponViewModel>> GetCouponByIdAsync(string couponId);

    Task<ResultViewModel<GetCouponViewModel>> GetCouponByCodeAsync(string couponCode);

    Task<ResultViewModel<string>> CreateCouponAsync(CreateCouponViewModel createCoupon);

    Task<ResultViewModel> UpdateCouponAsync(string id, UpdateCouponViewModel updateCoupon);

    Task<ResultViewModel> DeleteCouponAsync(string id, DeleteCouponViewModel deleteCoupon);

    Task<CollectionResultViewModel<bool>> DeleteCouponsAsync(DeleteCouponsViewModel deleteCoupons);
}