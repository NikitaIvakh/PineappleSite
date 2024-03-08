namespace PineappleSite.Presentation.Services.Favorites
{
    public partial class FavoritesClient : IFavoritesClient
    {
        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }
    }
}