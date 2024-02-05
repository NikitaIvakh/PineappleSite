using Favourites.Domain.DTOs;
using MediatR;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands
{
    public class ShoppingCartUpsertRequest : IRequest<Result<CartDto>>
    {
        public CartDto CartDto { get; set; }
    }
}