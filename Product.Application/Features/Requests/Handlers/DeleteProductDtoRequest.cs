using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Handlers
{
    public class DeleteProductDtoRequest : IRequest<Result<ProductDto>>
    {
        public DeleteProductDto DeleteProduct { get; set; }
    }
}