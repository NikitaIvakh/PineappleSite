using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.Application.Features.Requests.Handlers
{
    public class RemoveCartRequest : IRequest<Result<CartDetailsDto>>
    {
        public int CartDetailsId { get; set; }
    }
}