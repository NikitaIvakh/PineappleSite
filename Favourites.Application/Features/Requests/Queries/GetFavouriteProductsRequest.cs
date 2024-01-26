using MediatR;

namespace Favourites.Application.Features.Requests.Queries
{
    public class GetFavouriteProductsRequest : IRequest<GetFavouriteProductsRequest>
    {
        public string UserId { get; set; }
    }
}