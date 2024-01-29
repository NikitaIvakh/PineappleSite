using Favourites.Domain.DTOs;
using Favourites.Domain.ResultFavourites;
using MediatR;

namespace Favourites.Application.Features.Requests.Queries
{
    public class GetFavouriteProductsRequest : IRequest<Result<FavouritesDto>>
    {
        public string UserId { get; set; }
    }
}