using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands
{
    public class RemoveShoppingCartDetailsRequest : IRequest<Result<CartDetailsDto>>
    {
        public int CartDetailsId { get; set; }
    }
}