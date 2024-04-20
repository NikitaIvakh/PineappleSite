namespace PineappleSite.Presentation.Services.Favourite;

public sealed partial class FavouriteClient : IFavouriteClient
{
    public HttpClient HttpClient { get; }
}