﻿using AutoMapper;
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
    public class DeleteCouponListRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMapper mapper, DeleteListValidator validationRules, IMemoryCache memoryCache) : IRequestHandler<DeleteCouponListRequest, CollectionResult<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly DeleteListValidator _validator = validationRules;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "couponsCacheKey";

        public async Task<CollectionResult<CouponDto>> Handle(DeleteCouponListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _validator.ValidateAsync(request.DeleteCoupon, cancellationToken);

                if (!result.IsValid)
                {
                    return new CollectionResult<CouponDto>
                    {
                        ErrorMessage = ErrorMessage.CouponNotDeletedListCatch,
                        ErrorCode = (int)ErrorCodes.CouponNotDeletedListCatch,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var coupons = await _repository.GetAllAsync().Where(key => request.DeleteCoupon.CouponIds.Contains(key.CouponId)).ToListAsync(cancellationToken);

                    if (coupons.Count == 0)
                    {
                        return new CollectionResult<CouponDto>
                        {
                            ErrorMessage = ErrorMessage.CouponsNotFound,
                            ErrorCode = (int)ErrorCodes.CouponsNotFound,
                        };
                    }

                    else
                    {
                        await _repository.DeleteListAsync(coupons);

                        foreach (var coupon in coupons)
                        {
                            _memoryCache.Remove(coupon);
                        }

                        var couponsCache = await _repository.GetAllAsync().ToListAsync(cancellationToken);
                        _memoryCache.Remove(cacheKey);
                        _memoryCache.Set(cacheKey, coupons);

                        return new CollectionResult<CouponDto>
                        {
                            Count = coupons.Count,
                            SuccessMessage = "Купоны успешно удалены",
                            Data = _mapper.Map<IReadOnlyCollection<CouponDto>>(coupons),
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                return new CollectionResult<CouponDto>
                {
                    ErrorMessage = ErrorMessage.CouponNotDeletedListCatch,
                    ErrorCode = (int)ErrorCodes.CouponNotDeleted,
                };
            }
        }
    }
}