namespace Favourite.Domain.DTOs;

public sealed class FavouriteDetailsDto
{
    public int FavouriteDetailsId { get; set; }

    public FavouriteHeaderDto? FavouriteHeader { get; init; }

    public int FavouriteHeaderId { get; set; }

    public ProductDto? Product { get; set; }

    public int ProductId { get; init; }
}