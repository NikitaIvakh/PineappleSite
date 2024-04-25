using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Handlers;

public sealed class CreateProductRequest(CreateProductDto createProduct) : IRequest<Result<int>>
{
    public CreateProductDto CreateProduct { get; init; } = createProduct;
}