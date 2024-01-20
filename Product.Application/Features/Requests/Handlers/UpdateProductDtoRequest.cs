using MediatR;
using Product.Application.DTOs.Products;

namespace Product.Application.Features.Requests.Handlers
{
    public class UpdateProductDtoRequest : IRequest<Unit>
    {
        public UpdateProductDto UpdateProduct { get; set; }
    }
}