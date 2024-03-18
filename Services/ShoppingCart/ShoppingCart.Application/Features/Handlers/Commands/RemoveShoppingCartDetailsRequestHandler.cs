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
    public class RemoveShoppingCartDetailsRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IBaseRepository<CartDetails> cartDetailsRepository, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<RemoveShoppingCartDetailsRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetailsRepository;
        private readonly IMapper _mapper = mapper;

        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheGetShoppingCartKey";

        public async Task<Result<CartHeaderDto>> Handle(RemoveShoppingCartDetailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartDetails? cartDetails = _cartDetailsRepository.GetAll().FirstOrDefault(key => key.ProductId == request.ProductId);
                CartHeader? cartHeader = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.CartHeaderId == cartDetails.CartHeaderId);

                if (cartDetails is null)
                {
                    return new Result<CartHeaderDto>
                    {
                        ErrorMessage = ErrorMessages.DetailsNotFound,
                        ErrorCode = (int)ErrorCodes.DetailsNotFound,
                    };
                }

                else
                {
                    int totalRemoveCartDetails = _cartDetailsRepository.GetAll().Where(key => key.CartHeaderId == cartDetails.CartHeaderId).Count();
                    await _cartDetailsRepository.DeleteAsync(cartDetails);

                    if (totalRemoveCartDetails == 1)
                    {
                        CartHeader? cartHeaderDelete = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.CartHeaderId == cartDetails.CartHeaderId);

                        if (cartHeader is not null)
                        {
                            await _cartHeaderRepository.DeleteAsync(cartHeader);
                        }
                    }

                    var getAllheaders = _cartHeaderRepository.GetAll().ToList();
                    var getAlldetails = _cartDetailsRepository.GetAll().ToList();

                    _memoryCache.Remove(getAllheaders);
                    _memoryCache.Remove(getAlldetails);

                    _memoryCache.Set(cacheKey, getAllheaders);
                    _memoryCache.Set(cacheKey, getAlldetails);

                    return new Result<CartHeaderDto>
                    {
                        Data = _mapper.Map<CartHeaderDto>(cartHeader),
                        SuccessMessage = "Продукт успешно удален",
                    };
                }
            }

            catch (Exception exception)
            {
                return new Result<CartHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message }
                };
            }
        }
    }
}