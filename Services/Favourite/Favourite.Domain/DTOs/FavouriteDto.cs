namespace Favourite.Domain.DTOs
{
    public class FavouriteDto
    {
        public FavouriteHeaderDto FavouriteHeader { get; set; }

        public List<FavouriteDetailsDto> FavouriteDetails { get; set; }
    }
}