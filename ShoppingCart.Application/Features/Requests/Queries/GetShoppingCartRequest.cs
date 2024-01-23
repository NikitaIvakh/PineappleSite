using MediatR;
using ShoppingCart.Application.Response;

namespace ShoppingCart.Application.Features.Requests.Queries
{
    public class GetShoppingCartRequest : IRequest<ShoppingCartAPIResponse>
    {
        public string UserId { get; set; }
    }
}