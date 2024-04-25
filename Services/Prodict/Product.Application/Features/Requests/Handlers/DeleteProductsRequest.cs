using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Handlers;

public sealed class DeleteProductsRequest(DeleteProductsDto deleteProducts) : IRequest<CollectionResult<bool>>
{
    public DeleteProductsDto DeleteProducts { get; init; } = deleteProducts;
}