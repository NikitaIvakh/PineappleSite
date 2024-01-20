using MediatR;
using Product.Application.DTOs.Products;

namespace Product.Application.Features.Requests.Handlers
{
    public class DeleteProductDtoRequest : IRequest<Unit>
    {
        public DeleteProductDto DeleteProduct { get; set; }
    }
}