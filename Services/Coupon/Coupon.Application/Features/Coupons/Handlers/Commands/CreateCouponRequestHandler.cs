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
using Stripe;

namespace Coupon.Application.Features.Coupons.Handlers.Commands;

public sealed class CreateCouponRequestHandler(
    ICouponRepository repository,
    CreateValidator createValidator,
    IMemoryCache memoryCache)
    : IRequestHandler<CreateCouponRequest, Result<string>>
{
    private const string CacheKey = "couponsCacheKey";

    public async Task<Result<string>> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await createValidator.ValidateAsync(request.CreateCoupon, cancellationToken);

            if (!result.IsValid)
            {
                var errorMessages = new Dictionary<string, List<string>>
                {
                    { "CouponCode", result.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "DiscountAmount", result.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "MinAmount", result.Errors.Select(key => key.ErrorMessage).ToList() }
                };

                foreach (var error in errorMessages)
                {
                    if (errorMessages.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new Result<string>
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("CouponNotCreated", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("CouponNotCreated", ErrorMessage.Culture),
                };
            }

            var couponAlreadyExists = await repository.GetAllAsync()
                .FirstOrDefaultAsync(key => key.CouponCode == request.CreateCoupon.CouponCode, cancellationToken);

            if (couponAlreadyExists is not null)
            {
                return new Result<string>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("CouponAlreadyExists", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("CouponAlreadyExists", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var coupon = new CouponEntity
            {
                CouponCode = request.CreateCoupon.CouponCode.Replace(" ", ""),
                DiscountAmount = request.CreateCoupon.DiscountAmount,
                MinAmount = request.CreateCoupon.MinAmount,
            };

            await repository.CreateAsync(coupon);

            var options = new CouponCreateOptions
            {
                Currency = "byn",
                Id = coupon.CouponCode,
                Name = coupon.CouponCode,
                AmountOff = (long)(coupon.DiscountAmount * 100),
            };

            CouponService? service = new();

            if (options.Id is not null)
            {
                try
                {
                    var existingCoupon =
                        await service?.GetAsync(coupon.CouponCode, cancellationToken: cancellationToken)!;

                    if (existingCoupon is not null)
                    {
                        await service?.DeleteAsync(coupon.CouponCode, cancellationToken: cancellationToken)!;
                        await service?.CreateAsync(options, cancellationToken: cancellationToken)!;
                    }
                }

                catch (StripeException ex)
                {
                    await service?.CreateAsync(options, cancellationToken: cancellationToken)!;
                }
            }

            memoryCache.Remove(CacheKey);
            var coupons = await repository.GetAllAsync().ToListAsync(cancellationToken);

            memoryCache.Set(CacheKey, coupon);
            memoryCache.Set(CacheKey, coupons);

            return new Result<string>
            {
                Data = coupon.CouponId,
                StatusCode = (int)StatusCode.Created,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponSuccessfullyCreated", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new Result<string>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}