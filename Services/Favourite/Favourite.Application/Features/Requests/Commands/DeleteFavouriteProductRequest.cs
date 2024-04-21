using MediatR;
using Favourite.Domain.Results;

namespace Favourite.Application.Features.Requests.Commands;

public sealed class DeleteFavouriteProductRequest(int productId) : IRequest<Result<Unit>>
{
    public int ProductId { get; } = productId;
}