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
    public class DeleteCouponRequestHandler(ICouponRepository repository, DeleteValidator validator, IMemoryCache memoryCache) 
        : IRequestHandler<DeleteCouponRequest, Result<Unit>>
    {
        private const string CacheKey = "couponsCacheKey";

        public async Task<Result<Unit>> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await validator.ValidateAsync(request.DeleteCouponDto, cancellationToken);

                if (!result.IsValid)
                {
                    var existErrorMessages = new Dictionary<string, List<string>>
                    {
                        { "CouponId", result.Errors.Select(key => key.ErrorMessage).ToList() },
                    };

                    foreach (var error in existErrorMessages)
                    {
                        if (existErrorMessages.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<Unit>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.CouponNotDeleted,
                                ErrorCode = (int)ErrorCodes.CouponNotDeleted,
                            };
                        }
                    }

                    return new Result<Unit>
                    {
                        ErrorMessage = ErrorMessage.CouponNotDeleted,
                        ErrorCode = (int)ErrorCodes.CouponNotDeleted,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                var coupon = await repository.GetAllAsync()
                    .FirstOrDefaultAsync(key => key.CouponId == request.DeleteCouponDto.CouponId,
                        cancellationToken);

                if (coupon is null)
                {
                    return new Result<Unit>
                    {
                        ErrorMessage = ErrorMessage.CouponNotFound,
                        ErrorCode = (int)ErrorCodes.CouponNotFound,
                        ValidationErrors = [ErrorMessage.CouponNotFound]
                    };
                }

                await repository.DeleteAsync(coupon);
                var couponCache = await repository.GetAllAsync().ToListAsync(cancellationToken);

                memoryCache.Remove(coupon);
                memoryCache.Remove(couponCache);

                memoryCache.Set(CacheKey, couponCache);

                return new Result<Unit>
                {
                    Data = Unit.Value,
                    SuccessCode = (int)SuccessCode.Deleted,
                    SuccessMessage = SuccessMessage.CouponDeletedSuccessfully,
                };
            }

            catch (Exception exception)
            {
                memoryCache.Remove(CacheKey);
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