using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands
{
    public class RemoveShoppingCartDetailsListRequest : IRequest<Result<CartHeaderDto>>
    {
        public DeleteProductList DeleteProduct { get; set; }
    }
}