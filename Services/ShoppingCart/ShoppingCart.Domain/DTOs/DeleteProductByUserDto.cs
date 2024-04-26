namespace ShoppingCart.Domain.DTOs;

public sealed class DeleteProductByUserDto(int productId, string userId)
{
    public int ProductId { get; set; } = productId;

    public string UserId { get; set; } = userId;
}