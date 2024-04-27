namespace Favourite.Domain.DTOs;

public class DeleteFavouriteProductByUserDto(int productId, string userId)
{
    public int ProductId { get; set; } = productId;
    
    public string UserId { get; set; } = userId;
}