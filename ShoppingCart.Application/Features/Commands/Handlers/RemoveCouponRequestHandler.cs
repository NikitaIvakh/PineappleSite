using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Domain.Entities.Cart;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.ResultCart;
using ShoppingCart.Domain.DTOs;
using Serilog;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Enum;

namespace ShoppingCart.Application.Features.Commands.Handlers
{
    public class RemoveCouponRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IMapper mapper, ILogger logger) : IRequestHandler<RemoveCouponRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger.ForContext<RemoveCouponRequestHandler>();

        public async Task<Result<CartHeaderDto>> Handle(RemoveCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartHeader? cartHeaderFromDb = await _cartHeaderRepository.GetAll().FirstOrDefaultAsync(key => key.UserId == request.CartDto.CartHeader.UserId, cancellationToken);

                if (cartHeaderFromDb is null)
                {
                    return new Result<CartHeaderDto>
                    {
                        ErrorMessage = ErrorMessages.CouponNotFound,
                        ErrorCode = (int)ErrorCodes.CouponNotFound,
                    };
                }

                else
                {
                    cartHeaderFromDb.CouponCode = string.Empty;
                    await _cartHeaderRepository.UpdateAsync(cartHeaderFromDb);

                    return new Result<CartHeaderDto>
                    {
                        Data = _mapper.Map<CartHeaderDto>(cartHeaderFromDb),
                        SuccessMessage = "Купон успешно удален",
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<CartHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}