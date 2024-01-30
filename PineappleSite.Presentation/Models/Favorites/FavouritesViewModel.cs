namespace PineappleSite.Presentation.Models.Favorites
{
    public class FavouritesViewModel
    {
        public FavoriteHeaderViewModel? FavoutiteHeader { get; set; }

        public IReadOnlyCollection<FavoriteDetailsViewModel>? FavouritesDetails { get; set; }
    }
}