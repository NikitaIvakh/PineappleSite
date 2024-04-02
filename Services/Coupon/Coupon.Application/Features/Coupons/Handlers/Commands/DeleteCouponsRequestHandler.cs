using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Resources;
using Coupon.Application.Validations;
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
                                StatusCode = (int)StatusCode.NoContent,
                                ErrorMessage = ErrorMessage.CouponNotDeletedListCatch,
                            };
                        }
                    }

                    return new CollectionResult<Unit>
                    {
                        StatusCode = (int)StatusCode.NoContent,
                        ErrorMessage = ErrorMessage.CouponNotDeletedListCatch,
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
                        StatusCode = (int)StatusCode.NotFound,
                        ErrorMessage = ErrorMessage.CouponsNotFound,
                        ValidationErrors = [ErrorMessage.CouponsNotFound]
                    };
                }

                foreach (var coupon in coupons)
                {
                    memoryCache.Remove(coupon);
                    await repository.DeleteAsync(coupon);
                }

                var couponsCache = await repository.GetAllAsync().ToListAsync(cancellationToken);
                memoryCache.Remove(CacheKey);
                memoryCache.Set(CacheKey, coupons);

                return new CollectionResult<Unit>
                {
                    Count = coupons.Count,
                    Data = new[] {Unit.Value},
                    StatusCode = (int)StatusCode.Deleted,
                    SuccessMessage = SuccessMessage.CouponsDeletedSuccessfully,
                };
            }

            catch (Exception exception)
            {
                return new CollectionResult<Unit>
                {
                    ErrorMessage = exception.Message,
                    StatusCode= (int)StatusCode.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}