using Favourite.Domain.DTOs;

namespace Favourite.Domain.Entities
{
    public class FavouriteDetails
    {
        public int FavouriteDetailsId { get; set; }

        public FavouriteHeader? FavouriteHeader { get; set; }

        public int FavouriteHeaderId { get; set; }

        public ProductDto? Product { get; set; }

        public int ProductId { get; set; }
    }
}