namespace PineappleSite.Presentation.Models.Favourites;

public sealed class FavouriteViewModel(
    FavouriteHeaderViewModel favouriteHeader,
    List<FavouriteDetailsViewModel> favouriteDetails)
{
    public FavouriteHeaderViewModel FavouriteHeader { get; set; } = favouriteHeader;

    public List<FavouriteDetailsViewModel> FavouriteDetails { get; set; } = favouriteDetails;
}