using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands
{
    public class ApplyCouponRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IMapper mapper, ICouponService couponService, IMemoryCache memoryCache) : IRequestHandler<ApplyCouponRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly ICouponService _couponService = couponService;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheGetShoppingCartKey";

        public async Task<Result<CartHeaderDto>> Handle(ApplyCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartHeader? cartHeader = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.UserId == request.CartDto.CartHeader.UserId);

                if (cartHeader is null)
                {
                    return new Result<CartHeaderDto>
                    {
                        ErrorCode = (int)ErrorCodes.CartHeaderNotFound,
                        ErrorMessage = ErrorMessages.CartHeaderNotFound,
                        ValidationErrors = [ErrorMessages.CartHeaderNotFound]
                    };
                }

                else
                {
                    var coupon = await _couponService.GetCouponAsync(request.CartDto.CartHeader.CouponCode);

                    if (coupon.Data is null)
                    {
                        return new Result<CartHeaderDto>
                        {
                            ErrorMessage = ErrorMessages.CouponNotFound,
                            Data = _mapper.Map<CartHeaderDto>(cartHeader),
                            ValidationErrors = [ErrorMessages.CouponNotFound]
                        };
                    }

                    else
                    {
                        cartHeader.CouponCode = request.CartDto.CartHeader.CouponCode.Trim();
                        await _cartHeaderRepository.UpdateAsync(cartHeader);

                        var getAllheaders = _cartHeaderRepository.GetAll().ToList();

                        _memoryCache.Remove(getAllheaders);
                        _memoryCache.Remove(cartHeader!);

                        _memoryCache.Set(cacheKey, getAllheaders);

                        return new Result<CartHeaderDto>
                        {
                            SuccessCode = (int)SuccessCode.Updated,
                            Data = _mapper.Map<CartHeaderDto>(cartHeader),
                            SuccessMessage = SuccessMessage.CouponSuccessfullyApplied,
                        };
                    }
                }
            }

            catch
            {
                return new Result<CartHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessages.InternalServerError]
                };
            }
        }
    }
}