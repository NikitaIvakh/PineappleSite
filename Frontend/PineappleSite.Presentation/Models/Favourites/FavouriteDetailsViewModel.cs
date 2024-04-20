using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models.Favourites;

public sealed class FavouriteDetailsViewModel
{
    public int FavouriteDetailsId { get; init; }

    public FavouriteHeaderViewModel? FavouriteHeader { get; init; }

    public int FavouriteHeaderId { get; init; }

    public ProductViewModel? Product { get; init; }

    public int ProductId { get; init; }
}