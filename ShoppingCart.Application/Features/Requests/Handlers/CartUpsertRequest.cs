using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.Application.Features.Requests.Handlers
{
    public class CartUpsertRequest : IRequest<Result<CartHeaderDto>>
    {
        public CartDto CartDto { get; set; }
    }
}