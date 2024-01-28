namespace Coupon.Domain.Enum
{
    public enum ErrorCodes
    {
        CouponAlreadyExists = 403,
        CouponNotFound = 404,
        CouponNotDeleted = 405,
        CouponNotCreated = 406,
        CouponNotUpdated = 304,
        InternalServerError = 500,
    }
}