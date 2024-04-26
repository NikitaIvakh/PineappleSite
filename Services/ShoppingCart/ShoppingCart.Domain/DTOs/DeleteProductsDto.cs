namespace ShoppingCart.Domain.DTOs;

public sealed class DeleteProductsDto(List<int> productIds)
{
    public List<int> ProductIds { get; set; } = productIds;
}