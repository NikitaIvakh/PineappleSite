using MediatR;
using ShoppingCart.Application.Response;

namespace ShoppingCart.Application.Features.Requests.Handlers
{
    public class RemoveCartRequest : IRequest<ShoppingCartAPIResponse>
    {
        public int CartDetailsId { get; set; }
    }
}