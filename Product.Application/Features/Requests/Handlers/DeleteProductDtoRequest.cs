using MediatR;
using Product.Application.DTOs.Products;
using Product.Application.Response;

namespace Product.Application.Features.Requests.Handlers
{
    public class DeleteProductDtoRequest : IRequest<ProductAPIResponse>
    {
        public DeleteProductDto DeleteProduct { get; set; }
    }
}