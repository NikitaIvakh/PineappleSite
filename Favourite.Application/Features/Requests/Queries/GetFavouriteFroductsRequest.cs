using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Queries
{
    public class GetFavouriteFroductsRequest : IRequest<Result<FavouriteDto>>
    {
        public string UserId { get; set; }
    }
}