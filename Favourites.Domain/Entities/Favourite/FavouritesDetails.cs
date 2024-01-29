using Favourites.Domain.DTOs;

namespace Favourites.Domain.Entities.Favourite
{
    public class FavouritesDetails
    {
        public int FavouritesDetailsId { get; set; }

        public FavouritesHeader FavouritesHeader { get; set; }

        public int FavouritesHeaderId { get; set; }

        public ProductDto Product { get; set; }

        public int ProductId { get; set; }
    }
}