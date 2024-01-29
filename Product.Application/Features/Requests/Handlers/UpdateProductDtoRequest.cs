using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Handlers
{
    public class UpdateProductDtoRequest : IRequest<Result<ProductDto>>
    {
        public UpdateProductDto UpdateProduct { get; set; }
    }
}