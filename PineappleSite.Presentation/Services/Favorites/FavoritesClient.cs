namespace PineappleSite.Presentation.Services.Favorites
{
    public partial class FavoritesClient : IFavoritesClient
    {
        HttpClient HttpClient
        {
            get { return _httpClient; }
        }
    }
}