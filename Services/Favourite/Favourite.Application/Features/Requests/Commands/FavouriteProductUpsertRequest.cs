using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Commands;

public sealed class FavouriteProductUpsertRequest(FavouriteDto favouriteDto) : IRequest<Result<FavouriteHeaderDto>>
{
    public FavouriteDto FavouriteDto { get; init; } = favouriteDto;
}