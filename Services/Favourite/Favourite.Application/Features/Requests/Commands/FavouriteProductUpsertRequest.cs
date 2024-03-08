using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Commands
{
    public class FavouriteProductUpsertRequest : IRequest<Result<FavouriteHeaderDto>>
    {
        public FavouriteDto FavouriteDto { get; set; }
    }
}