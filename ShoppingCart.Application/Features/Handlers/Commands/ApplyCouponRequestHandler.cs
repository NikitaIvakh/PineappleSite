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
    public class ApplyCouponRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IMapper mapper) : IRequestHandler<ApplyCouponRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CartHeaderDto>> Handle(ApplyCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartHeader? cartHeader = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.UserId == request.CartDto.CartHeader.UserId);

                if (cartHeader is null)
                {
                    return new Result<CartHeaderDto>
                    {
                        ErrorMessage = ErrorMessages.CartHeaderNotFound,
                        ErrorCode = (int)ErrorCodes.CartHeaderNotFound,
                    };
                }

                else
                {
                    cartHeader.CouponCode = request.CartDto.CartHeader.CouponCode;
                    await _cartHeaderRepository.UpdateAsync(cartHeader);

                    return new Result<CartHeaderDto>
                    {
                        Data = _mapper.Map<CartHeaderDto>(cartHeader),
                        SuccessMessage = "Купон успешно применен",
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