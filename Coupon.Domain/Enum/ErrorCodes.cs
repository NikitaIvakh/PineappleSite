namespace Coupon.Domain.Enum
{
    public enum ErrorCodes
    {
        CouponAlreadyExists = 403,
        CouponNotFound = 404,
        CouponsNotFound = 405,
        CouponNotDeleted = 406,
        CouponNotDeletedListCatch = 407,
        CouponNotCreated = 408,
        CouponNotUpdated = 304,
        CouponNotUpdatedNull = 305,
        InternalServerError = 500,
    }
}