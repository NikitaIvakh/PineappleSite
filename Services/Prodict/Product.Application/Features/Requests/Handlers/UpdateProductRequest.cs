using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Handlers;

public sealed class UpdateProductRequest(UpdateProductDto updateProduct) : IRequest<Result<Unit>>
{
    public UpdateProductDto UpdateProduct { get; set; } = updateProduct;
}