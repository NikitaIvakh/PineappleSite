using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Commands
{
    public class RemoveFavouriteProductRequest : IRequest<Result<FavouriteHeaderDto>>
    {
        public int ProductId { get; set; }
    }
}