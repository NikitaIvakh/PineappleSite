using AutoMapper;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Resources;
using Coupon.Application.Validations;
using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class UpdateCouponRequestHandler(IBaseRepository<CouponEntity> repository, UpdateValidator updateValidator, ILogger logger, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<UpdateCouponRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly UpdateValidator _updateValidator = updateValidator;
        private readonly ILogger _logger = logger.ForContext<UpdateCouponRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "couponsCacheKey";

        public async Task<Result<CouponDto>> Handle(UpdateCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _updateValidator.ValidateAsync(request.UpdateCoupon, cancellationToken);

                if (!result.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>
                    {
                        {"CouponCode", result.Errors.Select(key => key.ErrorMessage).ToList() },
                        {"DiscountAmount", result.Errors.Select(key => key.ErrorMessage).ToList() },
                        {"MinAmount", result.Errors.Select(key => key.ErrorMessage).ToList() },
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var errorMossage))
                        {
                            return new Result<CouponDto>
                            {
                                ValidationErrors = errorMossage,
                                ErrorMessage = ErrorMessage.CouponNotUpdated,
                                ErrorCode = (int)ErrorCodes.CouponNotUpdated,
                            };
                        }
                    }

                    return new Result<CouponDto>
                    {
                        ErrorMessage = ErrorMessage.CouponNotUpdated,
                        ErrorCode = (int)ErrorCodes.CouponNotUpdated,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var coupon = await _repository.GetAllAsync().FirstOrDefaultAsync(key => key.CouponId == request.UpdateCoupon.CouponId, cancellationToken);

                    if (coupon is null)
                    {
                        return new Result<CouponDto>
                        {
                            ErrorMessage = ErrorMessage.CouponNotUpdatedNull,
                            ErrorCode = (int)ErrorCodes.CouponNotUpdatedNull,
                        };
                    }

                    else
                    {
                        coupon.CouponCode = request.UpdateCoupon.CouponCode.Replace(" ", "");
                        coupon.DiscountAmount = request.UpdateCoupon.DiscountAmount;
                        coupon.MinAmount = request.UpdateCoupon.MinAmount;

                        await _repository.UpdateAsync(coupon);

                        var coupons = await _repository.GetAllAsync().ToListAsync(cancellationToken);

                        _memoryCache.Remove(coupon);
                        _memoryCache.Remove(coupons);

                        _memoryCache.Set(cacheKey, coupons);

                        return new Result<CouponDto>
                        {
                            Data = _mapper.Map<CouponDto>(coupon),
                            SuccessMessage = "Купон успешно обновлен",
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                return new Result<CouponDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}