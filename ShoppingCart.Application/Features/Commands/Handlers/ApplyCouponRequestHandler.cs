using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities.Cart;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.Application.Features.Commands.Handlers
{
    public class ApplyCouponRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IMapper mapper, ILogger logger) : IRequestHandler<ApplyCouponRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger.ForContext<ApplyCouponRequestHandler>();

        public async Task<Result<CartHeaderDto>> Handle(ApplyCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartHeader cartHeaderFromDb = await _cartHeaderRepository.GetAll().FirstAsync(key => key.UserId == request.CartDto.CartHeader.UserId, cancellationToken);
                cartHeaderFromDb.CouponCode = request.CartDto.CartHeader.CouponCode;
                await _cartHeaderRepository.UpdateAsync(cartHeaderFromDb);

                return new Result<CartHeaderDto>
                {
                    Data = _mapper.Map<CartHeaderDto>(cartHeaderFromDb),
                    SuccessMessage = "Купон успешно применен",
                };
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