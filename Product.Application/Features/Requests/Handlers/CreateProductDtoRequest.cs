using MediatR;
using Product.Application.DTOs.Products;

namespace Product.Application.Features.Requests.Handlers
{
    public class CreateProductDtoRequest : IRequest<int>
    {
        public CreateProductDto CreateProduct { get; set; }
    }
}