using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models
{
    public class ProductFavouriteViewModel
    {
        public ProductViewModel Product { get; set; }

        public FavouriteViewModel Favourite { get; set; }
    }
}