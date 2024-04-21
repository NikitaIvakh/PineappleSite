using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Commands;

public sealed class DeleteFavouriteProductsRequest(DeleteFavouriteProductsDto deleteFavourite)
    : IRequest<CollectionResult<Unit>>
{
    public DeleteFavouriteProductsDto DeleteFavourite { get; } = deleteFavourite;
}