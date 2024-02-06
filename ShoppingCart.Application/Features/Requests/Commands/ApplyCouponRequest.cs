using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands
{
    public class ApplyCouponRequest : IRequest<Result<CartHeaderDto>>
    {
        public CartHeaderDto CartHeaderDto { get; set; }
    }
}