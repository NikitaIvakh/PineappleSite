namespace PineappleSite.Presentation.Models.Favourites;

public sealed class DeleteFavouriteProductByUserViewModel(int productId, string userId)
{
    public int ProductId { get; init; } = productId;

    public string UserId { get; init; } = userId;
}