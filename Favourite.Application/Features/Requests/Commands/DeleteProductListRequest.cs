using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Commands
{
    public class DeleteProductListRequest : IRequest<Result<FavouriteHeaderDto>>
    {
        public DeleteFavouriteProducts DeleteFavourite { get; set; }
    }
}