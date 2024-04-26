using MediatR;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands;

public sealed class DeleteCouponsByCodeRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository)
    : IRequestHandler<DeleteCouponsByCodeRequest, CollectionResult<bool>>
{
    public async Task<CollectionResult<bool>> Handle(DeleteCouponsByCodeRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cartHeaderCoupons = cartHeaderRepository.GetAll()
                .Where(key => request.DeleteCouponsByCodeDto.CouponCodes.Contains(key.CouponCode!)).ToList();

            if (cartHeaderCoupons.Count == 0)
            {
                return new CollectionResult<bool>()
                {
                    StatusCode = (int)StatusCode.NoContent,
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

            return new CollectionResult<bool>()
            {
                Data = [true],
                Count = cartHeaderCoupons.Count,
                StatusCode = (int)StatusCode.Modify,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponsSuccessfullyUpdated", SuccessMessage.Culture)
            };
        }

        catch (Exception exception)
        {
            return new CollectionResult<bool>()
            {
                ErrorMessage = exception.Message,
                ValidationErrors = [exception.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}