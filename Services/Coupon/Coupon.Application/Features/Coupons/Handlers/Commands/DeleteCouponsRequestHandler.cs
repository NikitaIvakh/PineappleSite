using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Resources;
using Coupon.Application.Validations;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Coupon.Application.Features.Coupons.Handlers.Commands;

public sealed class DeleteCouponsRequestHandler(
    ICouponRepository repository,
    DeleteCouponsValidator validationRules,
    IMemoryCache memoryCache)
    : IRequestHandler<DeleteCouponsRequest, CollectionResult<bool>>
{
    private const string CacheKey = "couponsCacheKey";

    public async Task<CollectionResult<bool>> Handle(DeleteCouponsRequest request,
        CancellationToken cancellationToken)
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
                        return new CollectionResult<bool>
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage = ErrorMessage.ResourceManager.GetString("CouponNotDeletedListCatch",
                                ErrorMessage.Culture),
                        };
                    }
                }

                return new CollectionResult<bool>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("CouponNotDeletedListCatch", ErrorMessage.Culture),
                };
            }

            var coupons = await repository.GetAllAsync()
                .Where(key => request.DeleteCouponsDto.CouponIds.Contains(key.CouponId))
                .ToListAsync(cancellationToken);

            switch (coupons.Count)
            {
                case 0:
                    return new CollectionResult<bool>
                    {
                        StatusCode = (int)StatusCode.NotFound,
                        ErrorMessage = ErrorMessage.ResourceManager.GetString("CouponsNotFound", ErrorMessage.Culture),
                        ValidationErrors =
                        [
                            ErrorMessage.ResourceManager.GetString("CouponsNotFound", ErrorMessage.Culture) ??
                            string.Empty
                        ]
                    };
                
                case <= 1:
                    return new CollectionResult<bool>()
                    {
                        StatusCode = (int)StatusCode.NoContent,
                        ErrorMessage =
                            ErrorMessage.ResourceManager.GetString("ChooseOneOrMoreCoupons", ErrorMessage.Culture),
                        ValidationErrors =
                        [
                            ErrorMessage.ResourceManager.GetString("ChooseOneOrMoreCoupons", ErrorMessage.Culture) ??
                            string.Empty
                        ]
                    };
            }

            foreach (var coupon in coupons)
            {
                memoryCache.Remove(coupon);
                await repository.DeleteAsync(coupon);
            }

            var couponsCache = await repository.GetAllAsync().ToListAsync(cancellationToken);
            memoryCache.Remove(CacheKey);
            memoryCache.Set(CacheKey, couponsCache);

            return new CollectionResult<bool>
            {
                Count = coupons.Count,
                Data = new[] { true },
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponsDeletedSuccessfully", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<bool>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}