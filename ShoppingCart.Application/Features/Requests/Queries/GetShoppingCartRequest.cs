using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.Application.Features.Requests.Queries
{
    public class GetShoppingCartRequest : IRequest<Result<CartDto>>
    {
        public string UserId { get; set; }
    }
}