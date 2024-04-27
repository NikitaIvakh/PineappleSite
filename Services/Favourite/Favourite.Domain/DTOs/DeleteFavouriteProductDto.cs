namespace Favourite.Domain.DTOs;

public sealed class DeleteFavouriteProductDto(int id)
{
    public int Id { get; set; } = id;
}