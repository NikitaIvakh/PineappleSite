using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Queries;

public sealed class GetProductsRequest : IRequest<CollectionResult<GetProductsDto>>
{
}