using AutoMapper;
using Favourites.Domain.DTOs;
using MediatR;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands
{
    public class RemoveShoppingCartDetailsRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IBaseRepository<CartDetails> cartDetailsRepository, IMapper mapper) : IRequestHandler<RemoveShoppingCartDetailsRequest, Result<CartDetailsDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetailsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CartDetailsDto>> Handle(RemoveShoppingCartDetailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartDetails? cartDetails = _cartDetailsRepository.GetAll().FirstOrDefault(key => key.CartDetailsId == request.CartDetailsId);

                if (cartDetails is null)
                {
                    return new Result<CartDetailsDto>
                    {
                        ErrorMessage = ErrorMessages.InternalServerError,
                        ErrorCode = (int)ErrorCodes.InternalServerError,
                    };
                }

                else
                {
                    int totalRemoveCartDetails = _cartDetailsRepository.GetAll().Where(key => key.CartHeaderId == cartDetails.CartHeaderId).Count();
                    await _cartDetailsRepository.DeleteAsync(cartDetails);

                    if (totalRemoveCartDetails == 1)
                    {
                        CartHeader? cartHeader = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.CartHeaderId == cartDetails.CartHeaderId);

                        if (cartHeader is not null)
                        {
                            await _cartHeaderRepository.DeleteAsync(cartHeader);
                        }
                    }

                    return new Result<CartDetailsDto>
                    {
                        SuccessMessage = "Продукт успешно удален",
                        Data = _mapper.Map<CartDetailsDto>(cartDetails),
                    };
                }
            }

            catch (Exception exception)
            {
                return new Result<CartDetailsDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message }
                };
            }
        }
    }
}