namespace Favourite.Domain.DTOs;

public sealed class FavouriteDto(FavouriteHeaderDto favouriteHeader, List<FavouriteDetailsDto> favouriteDetails)
{
    public FavouriteHeaderDto FavouriteHeader { get; } = favouriteHeader;

    public List<FavouriteDetailsDto> FavouriteDetails { get; } = favouriteDetails;
}