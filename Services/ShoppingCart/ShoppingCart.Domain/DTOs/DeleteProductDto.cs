namespace ShoppingCart.Domain.DTOs;

public sealed class DeleteProductDto(int id)
{
    public int Id { get; set; } = id;
}