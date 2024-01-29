using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models.Favorites
{
    public class FavoriteDetailsViewModel
    {
        public int FavouritesDetailsId { get; set; }

        public FavoriteDetailsViewModel? FavouritesHeader { get; set; }

        public int FavouritesHeaderId { get; set; }

        public ProductViewModel? Product { get; set; }

        public int ProductId { get; set; }
    }
}