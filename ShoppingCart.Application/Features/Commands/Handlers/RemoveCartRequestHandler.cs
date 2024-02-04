using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.ResultCart;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Entities.Cart;
using Serilog;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Enum;

namespace ShoppingCart.Application.Features.Commands.Handlers
{
    public class RemoveCartRequestHandlerc(IBaseRepository<CartHeader> cartHeaderRepository, IBaseRepository<CartDetails> cartDetailsRepository, IMapper mapper, ILogger logger) : IRequestHandler<RemoveCartRequest, Result<CartDetailsDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetailsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger.ForContext<RemoveCartRequestHandlerc>();

        public async Task<Result<CartDetailsDto>> Handle(RemoveCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartDetails? cartDetails = await _cartDetailsRepository.GetAll().FirstOrDefaultAsync(key => key.Id == request.CartDetailsId, cancellationToken);

                if (cartDetails is null)
                {
                    return new Result<CartDetailsDto>
                    {
                        ErrorMessage = ErrorMessages.ProductNotFound,
                        ErrorCode = (int)ErrorCodes.ProductNotFound,
                    };
                }

                else
                {
                    int totalCartRemoveItems = await _cartDetailsRepository.GetAll().Where(key => key.CartHeaderId == cartDetails.CartHeaderId).CountAsync(cancellationToken);
                    await _cartDetailsRepository.DeleteAsync(cartDetails);

                    if (totalCartRemoveItems == 1)
                    {
                        CartHeader? cartHeader = await _cartHeaderRepository.GetAll().FirstOrDefaultAsync(key => key.Id == cartDetails.CartHeaderId, cancellationToken);

                        if (cartHeader is not null)
                            await _cartHeaderRepository.DeleteAsync(cartHeader);
                    }

                    return new Result<CartDetailsDto>
                    {
                        SuccessMessage = "Продукт из корзины успешно удален",
                        Data = _mapper.Map<CartDetailsDto>(cartDetails),
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<CartDetailsDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}