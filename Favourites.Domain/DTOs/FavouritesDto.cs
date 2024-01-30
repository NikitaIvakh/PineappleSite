using Favourites.Domain.ResultFavourites;

namespace Favourites.Domain.DTOs
{
    public class FavouritesDto
    {
        public FavouritesHeaderDto FavoutiteHeader { get; set; }

        public CollectionResult<FavouritesDetailsDto> FavouritesDetails { get; set; }
    }
}