using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Queries
{
    public class GetShoppingCartRequest : IRequest<Result<CartDto>>
    {
        public string UserId { get; set; }
    }
}