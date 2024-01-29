using MediatR;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Requests.Handlers
{
    public class CreateProductDtoRequest : IRequest<Result<ProductDto>>
    {
        public CreateProductDto CreateProduct { get; set; }
    }
}