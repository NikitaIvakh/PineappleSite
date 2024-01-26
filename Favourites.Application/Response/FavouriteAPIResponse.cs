namespace Favourites.Application.Response
{
    public class FavouriteAPIResponse
    {
        public int Id { get; set; }

        public object? Data { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = true;

        public List<string> ValidationErrors { get; set; }
    }
}