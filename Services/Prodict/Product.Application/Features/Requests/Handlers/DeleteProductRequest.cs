using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Handlers;

public sealed class DeleteProductRequest(DeleteProductDto deleteProduct) : IRequest<Result<Unit>>
{
    public DeleteProductDto DeleteProduct { get; init; } = deleteProduct;
}