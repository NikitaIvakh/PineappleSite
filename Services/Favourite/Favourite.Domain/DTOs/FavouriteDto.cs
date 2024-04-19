namespace Favourite.Domain.DTOs;

public sealed class FavouriteDto(FavouriteHeaderDto favouriteHeader, List<FavouriteDetailsDto> favouriteDetails)
{
    public FavouriteHeaderDto FavouriteHeader { get; init; } = favouriteHeader;

    public List<FavouriteDetailsDto> FavouriteDetails { get; init; } = favouriteDetails;
}