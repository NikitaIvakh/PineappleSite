using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Queries;

public sealed class GetProductRequest(int id) : IRequest<Result<ProductDto>>
{
    public int Id { get; init; } = id;
}