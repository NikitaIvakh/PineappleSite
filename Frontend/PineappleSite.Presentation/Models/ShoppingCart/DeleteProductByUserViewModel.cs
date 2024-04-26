namespace PineappleSite.Presentation.Models.ShoppingCart;

public sealed class DeleteProductByUserViewModel(int productId, string userId)
{
    public int ProductId { get; set; } = productId;

    public string UserId { get; set; } = userId;
}