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
    public class DeleteCouponsRequestHandler(ICouponRepository repository, DeleteCouponsValidator validationRules, IMemoryCache memoryCache) 
        : IRequestHandler<DeleteCouponsRequest, CollectionResult<Unit>>
    {
        private const string CacheKey = "couponsCacheKey";

        public async Task<CollectionResult<Unit>> Handle(DeleteCouponsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await validationRules.ValidateAsync(request.DeleteCouponsDto, cancellationToken);

                if (!result.IsValid)
                {
                    var existMessage = new Dictionary<string, List<string>>
                    {
                        { "CouponIds", result.Errors.Select(key => key.ErrorMessage).ToList() }
                    };

                    foreach (var error in existMessage)
                    {
                        if (existMessage.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new CollectionResult<Unit>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.CouponNotDeletedListCatch,
                                ErrorCode = (int)ErrorCodes.CouponNotDeletedListCatch,
                            };
                        }
                    }

                    return new CollectionResult<Unit>
                    {
                        ErrorMessage = ErrorMessage.CouponNotDeletedListCatch,
                        ErrorCode = (int)ErrorCodes.CouponNotDeletedListCatch,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                var coupons = await repository.GetAllAsync()
                    .Where(key => request.DeleteCouponsDto.CouponIds.Contains(key.CouponId))
                    .ToListAsync(cancellationToken);

                if (coupons.Count == 0)
                {
                    return new CollectionResult<Unit>
                    {
                        ErrorMessage = ErrorMessage.CouponsNotFound,
                        ErrorCode = (int)ErrorCodes.CouponsNotFound,
                        ValidationErrors = [ErrorMessage.CouponsNotFound]
                    };
                }

                await repository.DeleteListAsync(coupons);

                foreach (var coupon in coupons)
                {
                    memoryCache.Remove(coupon);
                }

                var couponsCache = await repository.GetAllAsync().ToListAsync(cancellationToken);
                memoryCache.Remove(CacheKey);
                memoryCache.Set(CacheKey, coupons);

                return new CollectionResult<Unit>
                {
                    Count = coupons.Count,
                    Data = new[] {Unit.Value},
                    SuccessCode = (int)SuccessCode.Deleted,
                    SuccessMessage = SuccessMessage.CouponsDeletedSuccessfully,
                };
            }

            catch (Exception exception)
            {
                return new CollectionResult<Unit>
                {
                    ErrorMessage = exception.Message,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}