namespace ShoppingCart.Domain.DTOs;

public sealed class DeleteProductDto(int productId)
{
    public int ProductId { get; set; } = productId;
}