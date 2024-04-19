using Favourite.Domain.DTOs;

namespace Favourite.Domain.Entities;

public sealed class FavouriteDetails
{
    public int FavouriteDetailsId { get; init; }

    public FavouriteHeader? FavouriteHeader { get; init; }

    public int FavouriteHeaderId { get; init; }

    public ProductDto? Product { get; init; }

    public int ProductId { get; init; }
}