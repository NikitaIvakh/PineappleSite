using Favourites.Application.DTOs;
using Favourites.Application.Response;
using MediatR;

namespace Favourites.Application.Features.Requests.Handlers
{
    public class RemoveFavoriteRequest : IRequest<FavouriteAPIResponse>
    {
        public int FavouriteDetailId { get; set; }
    }
}