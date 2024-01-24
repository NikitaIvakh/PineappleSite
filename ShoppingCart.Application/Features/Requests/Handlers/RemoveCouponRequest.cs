using MediatR;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Response;

namespace ShoppingCart.Application.Features.Requests.Handlers
{
    public class RemoveCouponRequest : IRequest<ShoppingCartAPIResponse>
    {
        public CartDto CartDto { get; set; }
    }
}