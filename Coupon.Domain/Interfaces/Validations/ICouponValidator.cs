using Coupon.Domain.Entities;
using Coupon.Domain.ResultCoupon;

namespace Coupon.Domain.Interfaces.Validations
{
    public interface ICouponValidator
    {
        Result ValidateCreate(CouponEntity coupon);

        Result ValidateUpdate(CouponEntity coupon);

        Result ValidateDelete(CouponEntity coupon);
    }
}