namespace PineappleSite.Presentation.Models.Favourites;

public sealed class DeleteFavouriteProductsViewModel(List<int> productIds)
{
    public List<int> ProductIds { get; init; } = productIds;
}