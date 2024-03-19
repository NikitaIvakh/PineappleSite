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
using Stripe;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class CreateCouponRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMapper mapper, CreateValidator createValidator, IMemoryCache memoryCache) : IRequestHandler<CreateCouponRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<CreateCouponRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly CreateValidator _createValidator = createValidator;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "couponsCacheKey";

        public async Task<Result<CouponDto>> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _createValidator.ValidateAsync(request.CreateCoupon, cancellationToken);

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
                            return new Result<CouponDto>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.CouponNotCreated,
                                ErrorCode = (int)ErrorCodes.CouponNotCreated,
                            };
                        }
                    }

                    return new Result<CouponDto>
                    {
                        ErrorMessage = ErrorMessage.CouponNotCreated,
                        ErrorCode = (int)ErrorCodes.CouponNotCreated,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var coupon = await _repository.GetAllAsync().FirstOrDefaultAsync(key => key.CouponCode == request.CreateCoupon.CouponCode, cancellationToken);

                    if (coupon is not null)
                    {
                        return new Result<CouponDto>
                        {
                            Data = null,
                            ErrorCode = (int)ErrorCodes.CouponAlreadyExists,
                            ErrorMessage = ErrorMessage.CouponAlreadyExists,
                            ValidationErrors = [ErrorMessage.CouponAlreadyExists]
                        };
                    }

                    else
                    {
                        coupon = new CouponEntity
                        {
                            CouponCode = request.CreateCoupon.CouponCode.Replace(" ", ""),
                            DiscountAmount = request.CreateCoupon.DiscountAmount,
                            MinAmount = request.CreateCoupon.MinAmount,
                        };

                        await _repository.CreateAsync(coupon);

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
                                var existingCoupon = service?.Get(coupon.CouponCode);

                                if (existingCoupon is not null)
                                {
                                    service?.Delete(coupon.CouponCode);
                                    service?.Create(options);
                                }
                            }

                            catch (StripeException ex)
                            {
                                service?.Create(options);
                            }
                        }

                        _memoryCache.Remove(cacheKey);
                        var coupons = await _repository.GetAllAsync().ToListAsync(cancellationToken);

                        _memoryCache.Set(cacheKey, coupon);
                        _memoryCache.Set(cacheKey, coupons);

                        return new Result<CouponDto>
                        {
                            Data = _mapper.Map<CouponDto>(coupon),
                            SuccessCode = (int)SuccessCode.Created,
                            SuccessMessage = SuccessMessage.CouponSuccessfullyCreated,
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                _memoryCache.Remove(cacheKey);
                return new Result<CouponDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}