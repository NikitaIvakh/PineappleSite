using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Models.Favorites
{
    public class FavouritesViewModel
    {
        public FavouritesHeaderViewModel FavoutiteHeader { get; set; }

        public IReadOnlyCollection<FavoriteDetailsViewModel> FavouritesDetails { get; set; }
    }
}