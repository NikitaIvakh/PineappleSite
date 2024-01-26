using Favourites.Application.Response;
using MediatR;

namespace Favourites.Application.Features.Requests.Queries
{
    public class GetFavouriteProductsRequest : IRequest<FavouriteAPIResponse>
    {
        public string UserId { get; set; }
    }
}