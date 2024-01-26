using Favourites.Domain.Entities.Favourite;

namespace Favourites.Application.DTOs
{
    public class FavouritesDetailsDto
    {
        public int FavouritesDetailsId { get; set; }

        public FavouritesHeader FavouritesHeader { get; set; }

        public int FavouritesHeaderId { get; set; }

        public ProductDto Product { get; set; }

        public int ProductId { get; set; }
    }
}