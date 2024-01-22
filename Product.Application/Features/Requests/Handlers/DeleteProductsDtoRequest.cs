using MediatR;
using Product.Application.DTOs.Products;
using Product.Application.Response;

namespace Product.Application.Features.Requests.Handlers
{
    public class DeleteProductsDtoRequest : IRequest<ProductAPIResponse>
    {
        public DeleteProductsDto DeleteProducts { get; set; }
    }
}