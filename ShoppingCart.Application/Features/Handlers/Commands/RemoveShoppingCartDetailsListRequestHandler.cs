using AutoMapper;
using MediatR;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands
{
    public class RemoveShoppingCartDetailsListRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IBaseRepository<CartDetails> cartDetailsRepository, IMapper mapper) : IRequestHandler<RemoveShoppingCartDetailsListRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetailsRepository;
        private readonly IMapper _mapper = mapper;

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

                    return new Result<CartHeaderDto>
                    {
                        SuccessMessage = "Продукты успешно удалены",
                        Data = _mapper.Map<CartHeaderDto>(cartHeader),
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