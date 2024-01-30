using Favourites.Domain.DTOs;
using Favourites.Domain.ResultFavourites;
using MediatR;

namespace Favourites.Application.Features.Requests.Handlers
{
    public class FavoutiteUpsertRequest : IRequest<Result<FavouritesHeaderDto>>
    {
        public FavouritesDto Favourites { get; set; }
    }
}