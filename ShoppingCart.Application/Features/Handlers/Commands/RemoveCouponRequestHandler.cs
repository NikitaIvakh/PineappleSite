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
    public class RemoveCouponRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IMapper mapper) : IRequestHandler<RemoveCouponRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CartHeaderDto>> Handle(RemoveCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartHeader? cartHeader = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.UserId == request.CartDto.CartHeader.UserId);

                if (cartHeader is null)
                {
                    return new Result<CartHeaderDto>
                    {
                        ErrorMessage = ErrorMessages.InternalServerError,
                        ErrorCode = (int)ErrorCodes.InternalServerError,
                    };
                }

                else
                {
                    cartHeader.CouponCode = string.Empty;
                    await _cartHeaderRepository.UpdateAsync(cartHeader);

                    return new Result<CartHeaderDto>
                    {
                        SuccessMessage = "Купон успешно удален",
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