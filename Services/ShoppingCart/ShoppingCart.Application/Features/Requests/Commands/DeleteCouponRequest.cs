using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public class DeleteCouponRequest(DeleteCouponDto deleteCouponDto) : IRequest<Result<Unit>>
{
    public DeleteCouponDto DeleteCouponDto { get; set; } = deleteCouponDto;
}