using Favourites.Domain.DTOs;
using Favourites.Domain.Entities.Favourite;
using Favourites.Domain.ResultFavourites;
using MediatR;

namespace Favourites.Application.Features.Requests.Handlers
{
    public class RemoveFavoriteRequest : IRequest<Result<FavouritesDetails>>
    {
        public int FavouriteDetailId { get; set; }
    }
}