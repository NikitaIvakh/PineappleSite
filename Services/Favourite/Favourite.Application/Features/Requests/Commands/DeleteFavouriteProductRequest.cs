using Favourite.Domain.DTOs;
using MediatR;
using Favourite.Domain.Results;

namespace Favourite.Application.Features.Requests.Commands;

public sealed class DeleteFavouriteProductRequest(DeleteFavouriteProductDto deleteFavouriteProductDto)
    : IRequest<Result<Unit>>
{
    public DeleteFavouriteProductDto DeleteFavouriteProductDto { get; } = deleteFavouriteProductDto;
}