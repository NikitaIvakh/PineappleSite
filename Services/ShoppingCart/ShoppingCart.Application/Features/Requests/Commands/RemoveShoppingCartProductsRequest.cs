using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public sealed class RemoveShoppingCartProductsRequest(DeleteProductsDto deleteProductDto) : IRequest<CollectionResult<bool>>
{
    public DeleteProductsDto DeleteProductDto { get; init; } = deleteProductDto;
}