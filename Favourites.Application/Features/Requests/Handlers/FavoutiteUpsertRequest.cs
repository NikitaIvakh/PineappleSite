using Favourites.Application.DTOs;
using Favourites.Application.Response;
using MediatR;

namespace Favourites.Application.Features.Requests.Handlers
{
    public class FavoutiteUpsertRequest : IRequest<FavouriteAPIResponse>
    {
        public FavouritesDto Favourites { get; set; }
    }
}