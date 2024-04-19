namespace Favourite.Domain.DTOs;

public sealed class DeleteFavouriteProductsDto(List<int> productIds)
{
    public List<int> ProductIds { get; set; } = productIds;
}