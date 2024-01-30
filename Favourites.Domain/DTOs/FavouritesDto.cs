using Favourites.Domain.ResultFavourites;

namespace Favourites.Domain.DTOs
{
    public class FavouritesDto
    {
        public FavoutiteHeaderDto? FavoutiteHeader { get; set; }

        public CollectionResult<FavouritesDetailsDto>? FavouritesDetails { get; set; }
    }
}