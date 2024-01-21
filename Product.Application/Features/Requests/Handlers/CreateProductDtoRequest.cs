using MediatR;
using Product.Application.DTOs.Products;
using Product.Application.Response;

namespace Product.Application.Features.Requests.Handlers
{
    public class CreateProductDtoRequest : IRequest<ProductAPIResponse>
    {
        public CreateProductDto CreateProduct { get; set; }
    }
}