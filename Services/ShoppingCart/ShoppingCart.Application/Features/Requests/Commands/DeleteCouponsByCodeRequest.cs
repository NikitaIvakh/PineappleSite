using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public class DeleteCouponsByCodeRequest(DeleteCouponsByCodeDto deleteCouponsByCodeDto) : IRequest<CollectionResult<bool>>
{
    public DeleteCouponsByCodeDto DeleteCouponsByCodeDto { get; set; } = deleteCouponsByCodeDto;
}