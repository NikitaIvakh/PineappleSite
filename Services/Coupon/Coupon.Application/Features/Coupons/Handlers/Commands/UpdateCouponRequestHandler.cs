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

public sealed class UpdateCouponRequestHandler(
    ICouponRepository repository,
    UpdateValidator updateValidator,
    IMemoryCache memoryCache)
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
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("CouponNotUpdated", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("CouponNotUpdated", ErrorMessage.Culture),
                };
            }

            var coupon = await repository.GetAllAsync()
                .FirstOrDefaultAsync(key => key.CouponId == request.UpdateCouponDto.CouponId, cancellationToken);

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

            coupon.CouponCode = request.UpdateCouponDto.CouponCode.Replace(" ", "").ToLower();
            coupon.DiscountAmount = request.UpdateCouponDto.DiscountAmount;
            coupon.MinAmount = request.UpdateCouponDto.MinAmount;

            await repository.UpdateAsync(coupon);
            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Modify,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponUpdatedSuccessfully", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}