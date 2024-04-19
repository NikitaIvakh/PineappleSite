namespace Favourite.Domain.Entities;

public sealed class FavouriteHeader
{
    public int FavouriteHeaderId { get; init; }

    public string? UserId { get; init; }
}