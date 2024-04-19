using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Queries;

public sealed class GetFavouriteProductsRequest(string userId) : IRequest<Result<FavouriteDto>>
{
    public string UserId { get; init; } = userId;
}