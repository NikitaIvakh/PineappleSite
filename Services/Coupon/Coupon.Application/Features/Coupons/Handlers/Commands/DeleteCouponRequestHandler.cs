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
    public class DeleteCouponRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMapper mapper, DeleteValidator validator, IMemoryCache memoryCache) : IRequestHandler<DeleteCouponRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<DeleteCouponRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly DeleteValidator _validator = validator;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "couponsCacheKey";

        public async Task<Result<CouponDto>> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _validator.ValidateAsync(request.DeleteCoupon, cancellationToken);

                if (!result.IsValid)
                {
                    var existErrorMessages = new Dictionary<string, List<string>>
                    {
                        {"Id", result.Errors.Select(key => key.ErrorMessage).ToList() },
                    };

                    foreach (var error in existErrorMessages)
                    {
                        if (existErrorMessages.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<CouponDto>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.CouponNotDeleted,
                                ErrorCode = (int)ErrorCodes.CouponNotDeleted,
                            };
                        }
                    }

                    return new Result<CouponDto>
                    {
                        ErrorMessage = ErrorMessage.CouponNotDeleted,
                        ErrorCode = (int)ErrorCodes.CouponNotDeleted,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var coupon = await _repository.GetAllAsync().FirstOrDefaultAsync(key => key.CouponId == request.DeleteCoupon.Id, cancellationToken);

                    if (coupon is null)
                    {
                        return new Result<CouponDto>
                        {
                            Data = null,
                            ErrorMessage = ErrorMessage.CouponNotFound,
                            ErrorCode = (int)ErrorCodes.CouponNotFound,
                            ValidationErrors = [ErrorMessage.CouponNotFound]
                        };
                    }

                    else
                    {
                        await _repository.DeleteAsync(coupon);
                        var couponCache = await _repository.GetAllAsync().ToListAsync(cancellationToken);

                        _memoryCache.Remove(coupon);
                        _memoryCache.Remove(couponCache);

                        _memoryCache.Set(cacheKey, couponCache);

                        return new Result<CouponDto>
                        {
                            Data = _mapper.Map<CouponDto>(coupon),
                            SuccessCode = (int)SuccessCode.Deleted,
                            SuccessMessage = SuccessMessage.CouponDeletedSuccessfully,
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