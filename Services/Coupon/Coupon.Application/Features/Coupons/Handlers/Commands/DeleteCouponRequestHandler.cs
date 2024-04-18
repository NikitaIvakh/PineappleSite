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

public class DeleteCouponRequestHandler(
    ICouponRepository repository,
    DeleteValidator validator,
    IMemoryCache memoryCache)
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
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("CouponNotDeleted", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("CouponNotDeleted", ErrorMessage.Culture),
                };
            }

            var coupon = await repository.GetAllAsync()
                .FirstOrDefaultAsync(key => key.CouponId == request.DeleteCouponDto.CouponId,
                    cancellationToken);

            if (coupon is null)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("CouponNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("CouponNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ]
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
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponDeletedSuccessfully", SuccessMessage.Culture),
            };
        }

        catch (Exception exception)
        {
            memoryCache.Remove(CacheKey);
            return new Result<Unit>
            {
                ErrorMessage = exception.Message,
                StatusCode = (int)StatusCode.InternalServerError,
                ValidationErrors = [ErrorMessage.InternalServerError]
            };
        }
    }
}