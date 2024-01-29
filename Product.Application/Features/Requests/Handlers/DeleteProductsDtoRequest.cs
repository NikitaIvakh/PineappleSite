using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Handlers
{
    public class DeleteProductsDtoRequest : IRequest<CollectionResult<ProductDto>>
    {
        public DeleteProductsDto DeleteProducts { get; set; }
    }
}