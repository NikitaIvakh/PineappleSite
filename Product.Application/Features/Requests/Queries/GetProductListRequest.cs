using MediatR;
using Product.Application.DTOs.Products;

namespace Product.Application.Features.Requests.Queries
{
    public class GetProductListRequest : IRequest<IReadOnlyCollection<ProductDto>>
    {

    }
}