using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Resources;
using Coupon.Application.Validations;
using Coupon.Domain.Entities;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class UpdateCouponRequestHandler(IBaseRepository<CouponEntity> repository, UpdateValidator updateValidator, IMemoryCache memoryCache) 
        : IRequestHandler<UpdateCouponRequest, Result<Unit>>
    {
        private const string CacheKey = "couponsCacheKey";

        public async Task<Result<Unit>> Handle(UpdateCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await updateValidator.ValidateAsync(request.UpdateCouponDto, cancellationToken);

                if (!result.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>
                    {
                        { "CouponId", result.Errors.Select(key => key.ErrorMessage).ToList() },
                        { "CouponCode", result.Errors.Select(key => key.ErrorMessage).ToList() },
                        { "DiscountAmount", result.Errors.Select(key => key.ErrorMessage).ToList() },
                        { "MinAmount", result.Errors.Select(key => key.ErrorMessage).ToList() },
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<Unit>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.CouponNotUpdated,
                                ErrorCode = (int)ErrorCodes.CouponNotUpdated,
                            };
                        }
                    }

                    return new Result<Unit>
                    {
                        ErrorMessage = ErrorMessage.CouponNotUpdated,
                        ErrorCode = (int)ErrorCodes.CouponNotUpdated,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                var coupon = await repository.GetAllAsync()
                    .FirstOrDefaultAsync(key => key.CouponId == request.UpdateCouponDto.CouponId, cancellationToken);

                if (coupon is null)
                {
                    return new Result<Unit>
                    {
                        ErrorMessage = ErrorMessage.CouponNotFound,
                        ErrorCode = (int)ErrorCodes.CouponNotFound,
                        ValidationErrors = [ErrorMessage.CouponNotFound]
                    };
                }

                coupon.CouponCode = request.UpdateCouponDto.CouponCode.Replace(" ", "");
                coupon.DiscountAmount = request.UpdateCouponDto.DiscountAmount;
                coupon.MinAmount = request.UpdateCouponDto.MinAmount;

                await repository.UpdateAsync(coupon);

                var coupons = await repository.GetAllAsync().ToListAsync(cancellationToken);

                memoryCache.Remove(coupon);
                memoryCache.Remove(coupons);

                memoryCache.Set(CacheKey, coupons);

                return new Result<Unit>
                {
                    Data = Unit.Value,
                    SuccessCode = (int)SuccessCode.Updated,
                    SuccessMessage = SuccessMessage.CouponUpdatedSuccessfully,
                };
            }

            catch (Exception exception)
            {
                return new Result<Unit>
                {
                    ErrorMessage = exception.Message,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}