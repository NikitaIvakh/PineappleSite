using MediatR;
using Product.Application.DTOs.Products;

namespace Product.Application.Features.Requests.Queries
{
    public class GetProductDetailsRequest : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }
}