namespace ShoppingCart.Domain.DTOs;

public sealed class CartDto(CartHeaderDto cartHeader, List<CartDetailsDto> cartDetails)
{
    public CartHeaderDto CartHeader { get; init; } = cartHeader;

    public List<CartDetailsDto> CartDetails { get; init; } = cartDetails;
}