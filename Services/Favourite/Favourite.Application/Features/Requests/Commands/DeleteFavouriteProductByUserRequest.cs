using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Commands;

public sealed class DeleteFavouriteProductByUserRequest(DeleteFavouriteProductByUserDto deleteFavouriteProductByUserDto)
    : IRequest<Result<Unit>>
{
    public DeleteFavouriteProductByUserDto DeleteFavouriteProductByUserDto { get; set; } =
        deleteFavouriteProductByUserDto;
}