using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models.Favourites
{
    public class FavouriteDetailsViewModel
    {
        public int FavouriteDetailsId { get; set; }

        public FavouriteHeaderViewModel? FavouriteHeader { get; set; }

        public int FavouriteHeaderId { get; set; }

        public ProductViewModel? Product { get; set; }

        public int ProductId { get; set; }
    }
}