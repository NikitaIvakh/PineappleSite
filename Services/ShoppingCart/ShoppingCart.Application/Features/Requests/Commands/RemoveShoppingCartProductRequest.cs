using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public sealed class RemoveShoppingCartProductRequest(DeleteProductDto deleteProductDto) : IRequest<Result<Unit>>
{
    public DeleteProductDto DeleteProductDto { get; init; } = deleteProductDto;
}