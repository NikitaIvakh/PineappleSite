using Favourites.Domain.DTOs;
using MediatR;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands
{
    public class ApplyCouponRequest : IRequest<Result<CartHeaderDto>>
    {
        public CartDto CartDto { get; set; }
    }
}