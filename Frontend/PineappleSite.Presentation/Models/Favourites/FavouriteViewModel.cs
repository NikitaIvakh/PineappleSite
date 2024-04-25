namespace PineappleSite.Presentation.Models.Favourites;

public sealed class FavouriteViewModel(
    FavouriteHeaderViewModel favouriteHeader,
    List<FavouriteDetailsViewModel> favouriteDetails)
{
    public FavouriteHeaderViewModel FavouriteHeader { get; init; } = favouriteHeader;

    public List<FavouriteDetailsViewModel> FavouriteDetails { get; init; } = favouriteDetails;
}