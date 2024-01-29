namespace Favourites.Domain.DTOs
{
    public class FavouritesDto
    {
        public FavoutiteHeaderDto FavoutiteHeader { get; set; }

        public IReadOnlyCollection<FavouritesDetailsDto> FavouritesDetails { get; set; }
    }
}