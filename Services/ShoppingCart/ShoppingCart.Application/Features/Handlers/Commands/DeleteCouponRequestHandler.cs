using AutoMapper;
using MediatR;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands;

public sealed class DeleteCouponRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IMapper mapper)
    : IRequestHandler<DeleteCouponRequest, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cartHeaderCoupons = cartHeaderRepository.GetAll()
                .Where(key => key.CouponCode == request.DeleteCouponDto.CouponCode).ToList();

            if (cartHeaderCoupons.Count == 0)
            {
                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("CouponsNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("CouponsNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }

            foreach (var cartHeader in cartHeaderCoupons)
            {
                cartHeader.CouponCode = null;
                await cartHeaderRepository.UpdateAsync(cartHeader);
            }

            return new Result<Unit>()
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Modify,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponsSuccessfullyUpdated", SuccessMessage.Culture)
            };
        }

        catch (Exception exception)
        {
            return new Result<Unit>()
            {
                ErrorMessage = exception.Message,
                ValidationErrors = [exception.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}