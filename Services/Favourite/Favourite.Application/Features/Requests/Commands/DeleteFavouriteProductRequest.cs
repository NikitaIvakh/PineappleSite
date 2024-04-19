using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Requests.Commands;

public sealed class DeleteFavouriteProductRequest(int productId) : IRequest<Result<FavouriteHeaderDto>>
{
    public int ProductId { get; init; } = productId;
}