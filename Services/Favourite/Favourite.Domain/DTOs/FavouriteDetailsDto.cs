namespace Favourite.Domain.DTOs
{
    public class FavouriteDetailsDto
    {
        public int FavouriteDetailsId { get; set; }

        public FavouriteHeaderDto? FavouriteHeader { get; set; }

        public int FavouriteHeaderId { get; set; }

        public ProductDto? Product { get; set; }

        public int ProductId { get; set; }
    }
}