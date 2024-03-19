using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands
{
    public class RemoveCouponRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<RemoveCouponRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheGetShoppingCartKey";

        public async Task<Result<CartHeaderDto>> Handle(RemoveCouponRequest request, CancellationToken cancellationToken)
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
                    cartHeader.CouponCode = null;
                    await _cartHeaderRepository.UpdateAsync(cartHeader);

                    var getAllheaders = _cartHeaderRepository.GetAll().ToList();

                    _memoryCache.Remove(getAllheaders);
                    _memoryCache.Remove(cartHeader!);
                    _memoryCache.Set(cacheKey, getAllheaders);

                    return new Result<CartHeaderDto>
                    {
                        SuccessCode = (int)SuccessCode.Deleted,
                        Data = _mapper.Map<CartHeaderDto>(cartHeader),
                        SuccessMessage = SuccessMessage.CouponSuccessfullyDeleted,
                    };
                }
            }

            catch
            {
                return new Result<CartHeaderDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ValidationErrors = [ErrorMessages.InternalServerError]
                };
            }
        }
    }
}