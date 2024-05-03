using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models;

public sealed class ProductFavouriteViewModel
{
    public ProductViewModel Product { get; init; }

    public FavouriteViewModel Favourite { get; init; }
}