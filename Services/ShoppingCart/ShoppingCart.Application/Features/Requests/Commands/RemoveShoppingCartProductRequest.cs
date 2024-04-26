using MediatR;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public sealed class RemoveShoppingCartProductRequest(int productId) : IRequest<Result<Unit>>
{
    public int ProductId { get; init; } = productId;
}