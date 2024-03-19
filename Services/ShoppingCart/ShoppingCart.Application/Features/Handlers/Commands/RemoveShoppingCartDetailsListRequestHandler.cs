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
    public class RemoveShoppingCartDetailsListRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IBaseRepository<CartDetails> cartDetailsRepository, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<RemoveShoppingCartDetailsListRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetailsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheGetShoppingCartKey";

        public async Task<Result<CartHeaderDto>> Handle(RemoveShoppingCartDetailsListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cartDetails = _cartDetailsRepository.GetAll().Where(key => request.DeleteProduct.ProductIds.Contains(key.ProductId)).ToList();
                var cartHeader = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.CartHeaderId == cartDetails.FirstOrDefault().CartHeaderId);

                if (cartDetails is null || cartDetails.Count == 0)
                {
                    return new Result<CartHeaderDto>
                    {
                        Data = new CartHeaderDto(),
                        ErrorCode = (int)ErrorCodes.DetailsNotFound,
                        ErrorMessage = ErrorMessages.DetailsNotFound,
                        ValidationErrors = [ErrorMessages.DetailsNotFound]
                    };
                }

                else
                {
                    int removeTotalDetails = _cartDetailsRepository.GetAll().Where(key => key.CartHeaderId == cartDetails.FirstOrDefault().CartHeaderId).Count();
                    await _cartDetailsRepository.DeleteListAsync(cartDetails);

                    if (removeTotalDetails == 1)
                    {
                        CartHeader? cartHeaderDelete = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.CartHeaderId == cartDetails.FirstOrDefault().CartHeaderId);

                        if (cartHeaderDelete is not null)
                        {
                            await _cartHeaderRepository.DeleteAsync(cartHeaderDelete);
                        }
                    }


                    var getAllheaders = _cartHeaderRepository.GetAll().ToList();
                    var getAlldetails = _cartDetailsRepository.GetAll().ToList();

                    _memoryCache.Remove(getAllheaders);
                    _memoryCache.Remove(getAlldetails);
                    _memoryCache.Remove(cartDetails);
                    _memoryCache.Remove(cartHeader!);

                    _memoryCache.Set(cacheKey, getAllheaders);
                    _memoryCache.Set(cacheKey, getAlldetails);

                    return new Result<CartHeaderDto>
                    {
                        SuccessCode = (int)SuccessCode.Deleted,
                        Data = _mapper.Map<CartHeaderDto>(cartHeader),
                        SuccessMessage = SuccessMessage.ProductsSuccessfullyDeleted,
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