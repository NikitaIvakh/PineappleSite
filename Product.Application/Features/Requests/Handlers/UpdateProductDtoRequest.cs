using MediatR;
using Product.Application.DTOs.Products;
using Product.Application.Response;

namespace Product.Application.Features.Requests.Handlers
{
    public class UpdateProductDtoRequest : IRequest<ProductAPIResponse>
    {
        public UpdateProductDto UpdateProduct { get; set; }
    }
}